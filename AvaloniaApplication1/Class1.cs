using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System;

namespace AvaloniaApplication1.Views;

public partial class Fader : UserControl
{

    static Pen LinePen = new Pen(new SolidColorBrush(Color.Parse("#5b5b5d")), 1);
    static Pen MiddleLinePen = new Pen(new SolidColorBrush(Color.Parse("#1d1d1f")), 3);

    static Pen FaderMiddleLinePen = new Pen(new SolidColorBrush(Color.Parse("#fdfdfe")), 1.5);
    static Typeface TextTypeface = new Typeface("Arial");
    const double LineLength = 12;
    const double FaderWidth = 23;
    const double FaderHeight = 41;

    public static readonly StyledProperty<IBrush> ForegroundProperty =
AvaloniaProperty.Register<Fader, IBrush>(nameof(Foreground), new SolidColorBrush(Color.Parse("#aa737277")));

    public IBrush Foreground
    {
        get
        {
            return GetValue(ForegroundProperty);
        }
        set
        {
            SetValue(ForegroundProperty, value);
        }
    }

    public static readonly StyledProperty<bool> ShowPopupProperty =
AvaloniaProperty.Register<Fader, bool>(nameof(ShowPopup), false);

    /// <summary>
    /// 推动推子时，是否显示数值
    /// </summary>
    public bool ShowPopup
    {
        get
        {
            return GetValue(ShowPopupProperty);
        }
        set
        {
            SetValue(ShowPopupProperty, value);
        }
    }

    public static readonly StyledProperty<float> VolumeProperty =
AvaloniaProperty.Register<Fader, float>(nameof(Volume), 1.0f, defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

    public float Volume
    {
        get
        {
            return GetValue(VolumeProperty);
        }
        set
        {
            SetValue(VolumeProperty, value);
        }
    }

    public static readonly StyledProperty<double> DbValueProperty =
      AvaloniaProperty.Register<Fader, double>(nameof(DbValue), 0.0);

    public double DbValue
    {
        get => GetValue(DbValueProperty);
        set => SetValue(DbValueProperty, value);
    }


    Rect _faderRect = new Rect();

    public Fader()
    {
        
        this.ClipToBounds = false;
        this.Focusable = true;

       
    }


    public double MovingDB;
    public Action BeginChange;
    public Action Changing;
    public Action EndChange;

    #region 鼠标事件
    PointerPoint _pressedPoint;
    bool _pressed;
    float _pressedStartVolume;
    double _pressedStartHeight;
    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        if (e.GetCurrentPoint(this).Properties.IsRightButtonPressed)
        {
            // 右键被按下
            return;
        }

        var point = e.GetCurrentPoint(this);
        if (_faderRect.Contains(point.Position) == false)
        {
            return;
        }


        if (e.ClickCount == 2)
        {
            if (ShowPopup)
            {
               
            }
            else
            {
                this.Volume = 1;
            }
        }
        else
        {
            e.Handled = true;
            e.Pointer.Capture(this);
            _pressedStartVolume = this.Volume;
            _pressedStartHeight = MathHelper.GetControlHeightByDB(MathHelper.GetDB(_pressedStartVolume), this.Bounds.Height, MathHelper.MINDB, MathHelper.MAXDB);
            _pressedPoint = point;
            _pressed = true;

            BeginChange?.Invoke();
           
        }
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);
        if (_pressed)
        {
            e.Handled = true;

            var point = e.GetCurrentPoint(this);

            var db = MathHelper.GetDbByHeight(_pressedStartHeight + _pressedPoint.Position.Y - point.Position.Y, this.Bounds.Height, MathHelper.MINDB, MathHelper.MAXDB);
            MovingDB = db;
            if (db <= MathHelper.MINDB)
                db = double.NegativeInfinity;
            else if (db > MathHelper.MAXDB)
                db = MathHelper.MAXDB;

            var volume = MathHelper.GetVolumeByDB(db);


            this.Volume = volume;
            Changing?.Invoke();
        }
    }


    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);

        if (_pressed)
        {
            e.Handled = true;
            _pressed = false;
            e.Pointer.Capture(null);
            EndChange?.Invoke();

            this.InvalidateVisual();

            
        }
    }
    #endregion

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == VolumeProperty)
        {
            this.InvalidateVisual();
            this.DbValue = Math.Round(MathHelper.GetDB((float)change.NewValue), 2);
        }
        else if (change.Property == ForegroundProperty)
        {
            this.InvalidateVisual();
        }
    }

    public override void Render(DrawingContext dc)
    {

        var width = this.Bounds.Width;
        var height = this.Bounds.Height;

        var db = MathHelper.MAXDB;
        double y, h;
        //画刻度
        while (db > MathHelper.MINDB)
        {
            h = MathHelper.GetControlHeightByDB(db, height, MathHelper.MINDB, MathHelper.MAXDB);

            y = height - h;
            if (db == 6 || db % 10 == 0)
            {
                dc.DrawLine(LinePen, new Point(width / 2 - LineLength, y), new Point(width / 2 - 1, y));
            }
            else
            {
                dc.DrawLine(LinePen, new Point(width / 2 - LineLength * 0.36, y), new Point(width / 2 - 1, y));
            }

            if (db == 6)
                db = 0;
            else
                db -= 5;
        }

        //画推子中间的竖线
        dc.DrawLine(MiddleLinePen, new Point(width / 2 + 7, -1), new Point(width / 2 + 7, height));

        db = MathHelper.GetDB(this.Volume);
        if (db > MathHelper.MAXDB)
            db = MathHelper.MAXDB;
        else if (db < MathHelper.MINDB)
            db = MathHelper.MINDB;

        h = MathHelper.GetControlHeightByDB(db, height, MathHelper.MINDB, MathHelper.MAXDB);

        y = height - h - FaderHeight / 2;
        var x = width / 2 + 7 - FaderWidth / 2;
        _faderRect = new Rect(x, y, FaderWidth, FaderHeight);
        //画推子
        dc.FillRectangle(this.Foreground, _faderRect, 5);

        //画推子中间横行
        dc.DrawLine(FaderMiddleLinePen, new Point(x + 5, y + FaderHeight / 2), new Point(x + FaderWidth - 5, y + FaderHeight / 2));
    }
}