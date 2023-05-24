# Hello World  

```C#    
Vz.Init();  // initialize vizzy context...
using (new OnStart())  // on flight start
{
    Vz.Display("Hello World!");
}
```  


# Event & Loop  

```C#  
using (new OnStart())
{
    using(new If(true))
    {
    }
    using (new Else())
    {
    }
    using (var loop = new For(1, 99, 1))
    {
        Vz.Log(loop.i);
    }
    using (new While(true))
    {
        Vz.WaitSeconds(0.01F);
        Vz.WaitUntil(false);
        Vz.Break();
    }
}
using (var evt = new OnReceiveMessage("Msg"))
{
    Vz.Log(evt.Data);
}
```  


# Variable/List Defination  

```C#  
Vz.Init();

var count = Vz.DeclareGlobal("count");
var list1 = Vz.DeclareListGlobal("list1");
using (new OnStart())
{
    count.Value = 3;
    list1.AddToList(1);
    list1.AddToList(2);
    list1.AddToList(3);
    // Or
    //Vz.InitList(list1, "1,2,3");
}
```  


# Custom Expressions/Custom Instructions  

```C#  
Vz.Init();

var CreateLabel = Vz.DeclareCustomInstruction("CreateLabel", "name", "text", "fontsize").SetInstructions((name, text, fontsize) => {
    VzLabel label = new VzLabel(name);
    label.FontSize = fontsize;
    label.Text = text;
})
using (new OnStart())
{
    CreateLabel("l1", "this is a label--1", 12);
    CreateLabel("l2", "this is a label--2", 12);
    CreateLabel("l3", "this is a label--3", 12);
}
```  

```C#  
Vz.Init();

var Clamp01 = Vz.DeclareCustomExpression("Clamp01", "value").SetReturn((value) => {
    using (new If(value > 1f))
    {
        value = 1f;
    }
    using (new ElseIf(value < 0f))
    {
        value = 0f;
    }
    return value;
})
using (new OnStart())
{
    Vz.Log(Clamp01(99f));
}
```  


# Commonly Used Methods    

```C#  
public static void Init(string name, bool requireMfd);
public static void Init();
public static UnaryExpression Abs(VzExpression value);
public static UnaryExpression ACos(VzExpression value);
public static VzInstruction ActivateStage();
public static UnaryExpression ASin(VzExpression value);
public static UnaryExpression ATan(VzExpression value);
public static VzInstruction Beep(VzExpression frequency, VzExpression volume, VzExpression time);
public static VzInstruction Break();
public static VzInstruction Broadcast(BroadCastType type, VzExpression msg, VzExpression data);
public static UnaryExpression Ceiling(VzExpression value);
public static VzInstruction CMT(string text);
public static ConditionalExpression Conditional(VzExpression condition, VzExpression e1, VzExpression e2);
public static UnaryExpression Cos(VzExpression value);
public static VzExpression CraftOtherProperty(VzExpression craftId, CraftOtherPropertyType property);
public static ListExpression CreateList(VzExpression values);
public static VzInstruction CreateWidget(WidgetType type, VzExpression name);
public static VzCustomExpression DeclareCustomExpression(string name, params string[] paramNames);
public static VzCustomInstruction DeclareCustomInstruction(string name, params string[] paramNames);
public static VzVariable DeclareGlobal(string varName);
public static VzListVariable DeclareListGlobal(string listName);
public static UnaryExpression Deg2Rad(VzExpression value);
public static VzInstruction DestroyAllWidget();
public static VzInstruction DestroyWidget(VzExpression widgetName);
public static VzInstruction Display(VzExpression text);
public static UnaryExpression Floor(VzExpression value);
public static FUNKExpression FUNK(FUNK funk);
public static VzExpression GetAG(VzExpression ag);
public static VzExpression GetCraftBaseProperty(CraftBasePropertyType property);
public static VzExpression GetCraftInput(CraftInput input);
public static GetGaugePropertyExpression GetGaugeProperty(VzExpression name, GaugePropertyType property);
public static VzVariable GetGlobal(string varName);
public static VzListVariable GetGlobalList(string listName);
public static GetLabelPropertyExpression GetLabelProperty(VzExpression name, LabelProperyType property);
public static GetPixelExpression GetPixel(VzExpression name, VzExpression x, VzExpression y);
public static GetSpritePropertyExpression GetSpriteProperty(VzExpression name, SpritePropertyType property);
public static GetWidgetEventMsgExpression GetWidgetEventMsg(VzExpression name, WidgetEventType eventType);
public static GetWidgePropertyExpression GetWidgetProperty(VzExpression name, WidgetPropertyGetType property);
public static HexColorExpression HexColor(VzExpression text);
public static UnaryExpression In(VzExpression value);
public static VzInstruction InitList(VzExpression list, VzExpression values);
public static VzInstruction InitTexture(VzExpression name, VzExpression width, VzExpression height);
public static VzExpression LatLongAglToPos(VzExpression location);
public static UnaryExpression Lg10(VzExpression value);
public static VzInstruction LockNavSphere(LockNavSphereIndicatorType type, VzExpression value);
public static VzInstruction Log(VzExpression text);
public static BinaryOp Max(VzExpression a, VzExpression b);
public static BinaryOp Min(VzExpression a, VzExpression b);
public static VzOnChangeSoiEvent OnChangeSoiBegin();
public static VzOnDockedEvent OnDockedBegin();
public static VzOnPartCollisionEvent OnPartCollisionBegin();
public static VzOnPartExplodeEvent OnPartExplodeBegin();
public static VzOnReceiveMessageEvent OnReceiveMessageBegin(string msg);
public static VzOnStartEvent OnStartBegin();
public static VzExpression PartLocalToPci(VzExpression partId, VzExpression coords);
public static VzExpression PartNameToID(VzExpression partName);
public static VzExpression PartPciToLocal(VzExpression partId, VzExpression coords);
public static VzExpression PartProperty(VzExpression partID, PartPropertyGetType propertyGet);
public static VzExpression PlanetProperty(VzExpression planet, PlanetPropertyType property);
public static VzExpression PosToLatLongAgl(VzExpression pos);
public static UnaryExpression Rad2Deg(VzExpression value);
public static UnaryExpression Round(VzExpression value);
public static VzInstruction SetAG(VzExpression ag, VzExpression newvalue);
public static VzInstruction SetCameraProperty(CameraProperty property, VzExpression newvalue);
public static VzInstruction SetGauge(VzExpression name, GaugePropertyType property, VzExpression value);
public static VzInstruction SetInput(CraftInput input, VzExpression value);
public static VzInstruction SetLabel(VzExpression name, LabelProperyType property, VzExpression value);
public static VzInstruction SetLabelAlignment(VzExpression name, Alignment alignment);
public static VzInstruction SetLine(VzExpression name, LinePropertyType property, VzExpression value);
public static VzInstruction SetLineStartEnd(VzExpression name, VzExpression start, VzExpression end);
public static VzInstruction SetMap(VzExpression name, MapPropertyType property, VzExpression value);
public static VzInstruction SetNavBall(VzExpression name, NavBallPropertyType property, VzExpression value);
public static VzInstruction SetPartProperty(VzExpression partId, PartPropertySetType setProperty, VzExpression newvalue);
public static VzInstruction SetPixel(VzExpression name, VzExpression x, VzExpression y, VzExpression color);
public static VzInstruction SetSprite(VzExpression name, SpritePropertyType property, VzExpression value);
public static VzInstruction SetTarget(VzExpression value);
public static VzInstruction SetTargetHeading(TargetHeadingProperty property, VzExpression value);
public static VzInstruction SetTimeMode(TimeMode mode);
public static VzInstruction SetWidget(VzExpression name, WidgetPropertyType property, VzExpression value);
public static VzInstruction SetWidgetAnchor(VzExpression name, WidgetAnchor anchor);
public static UnaryExpression Sin(VzExpression value);
public static UnaryExpression Sqrt(VzExpression value);
public static VzExpression StringContains(VzExpression stringA, VzExpression stringB);
public static VzExpression StringFormat(params VzExpression[] strings);
public static VzExpression StringFriend(VzExpression expr, PhysicalQuantity quantity);
public static VzExpression StringJoin(params VzExpression[] strings);
public static VzExpression StringLength(VzExpression str);
public static VzExpression StringLetter(VzExpression idx, VzExpression str);
public static VzExpression SubString(VzExpression start, VzExpression end, VzExpression str);
public static VzInstruction SwitchCraft(VzExpression craftId);
public static UnaryExpression Tan(VzExpression value);
public static VzExpression TerrainQuery(TerrainPropertyType property, VzExpression location);
public static VzInstruction WaitSeconds(VzExpression sec);
public static VzInstruction WaitUntil(VzExpression condition);
public static WidgetDisplayToLocalExpression WidgetDisplayToLocal(VzExpression name, VzExpression pos);
public static VzInstruction WidgetEvent(VzExpression widgetName, WidgetEventType eventType, VzExpression message, VzExpression data);
public static WidgetLocalToDisplayExpression WidgetLocalToDisplay(VzExpression name, VzExpression pos);
public static VzInstruction WidgetSendBack(VzExpression name, VzExpression target);
public static VzInstruction WidgetSendFront(VzExpression name, VzExpression target);

//for short  
public static VzExpression Get(TerrainPropertyType propertyType, VzExpression location);
public static VzExpression Get(CraftBasePropertyType property);
public static VzExpression Get(CraftInput input);
public static VzExpression Get(PartPropertyGetType propertyGet, VzExpression partId);
public static VzExpression Get(PlanetPropertyType propertyType, VzExpression planet);
public static VzExpression Get(CraftOtherPropertyType propertyType, VzExpression craftId);
public static VzInstruction Set(CraftInput input, VzExpression value);
public static VzInstruction Set(TargetHeadingProperty property, VzExpression value);
public static VzInstruction Set(CameraProperty property, VzExpression value);
public static VzInstruction Set(PartPropertySetType partPropertySet, VzExpression partId, VzExpression value);
```


# Proxy Classes  

```C#  
public class VzString : VizzyProxy
{
    public static VzExpression Letter(VzExpression idx, VzExpression str);
    public static VzExpression LengthOf(VzExpression str);
    public static VzExpression Join(params VzExpression[] strings);
    public static VzExpression SubString(VzExpression start, VzExpression end, VzExpression str);
    public static VzExpression Contains(VzExpression stringA, VzExpression stringB);
    public static VzExpression Format(params VzExpression[] strings);
    public static VzExpression Friend(VzExpression expr, PhysicalQuantity quantity);
}

public class VzCraft : VizzyProxy
{
    public VzExpression Altitude;
    public VzExpression Destroyed;
    public VzExpression Grounded;
    public VzExpression Mass;
    public VzExpression CraftName;
    public VzExpression PartCount;
    public VzExpression Planet;
    public VzExpression Position;
    public VzExpression Velocity;
    public VzExpression IsPlayer;
    public VzExpression Apoapsis;
    public VzExpression Periapsis;
    public VzExpression Period;
    public VzExpression ApoapsisTime;
    public VzExpression PeriapsisTime;
    public VzExpression Inclination;
    public VzExpression Eccentricity;
    public VzExpression MeanAnomaly;
    public VzExpression MeanMotion;
    public VzExpression PeriapsisArgument;
    public VzExpression RightAscension;
    public VzExpression TrueAnomaly;
    public VzExpression SemiMajorAxis;
    public VzExpression SemiMinorAxis;
    public static void ActivateStage();
    //Input Get/Set  
    public static VzExpression Roll;
    public static VzExpression Pitch;
    public static VzExpression Yaw;
    public static VzExpression Brake;
    public static VzExpression Throttle;
    public static VzExpression TranslateForward;
    public static VzExpression TranslateMode;
    public static VzExpression Slider1;
    public static VzExpression Slider2;
    public static VzExpression Slider3;
    public static VzExpression Slider4;
    public static VzExpression AG1;
    public static VzExpression AG2;
    public static VzExpression AG3;
    public static VzExpression AG4;
    public static VzExpression AG5;
    public static VzExpression AG6;
    public static VzExpression AG7;
    public static VzExpression AG8;
    public static VzExpression AG9;
    public static VzExpression AG10;
    //Target Heading Set  
    public static VzExpression TargetHeading_Heading;
    public static VzExpression TargetHeading_Pitch;
    public static VzExpression TargetHeading_Pid_Pitch;
    public static VzExpression TargetHeading_Pid_Roll;
    public static VzExpression Target;
    //LockNavSphere Set
    public static VzExpression LockNavSphereVector;
    public static LockNavSphereIndicatorType LockNavSphere;
    //Time Set
    public static TimeMode TimeMode
    //Camera Set  
    public static VzExpression CameraMode
    public static VzExpression CameraModeIndex
    public static VzExpression CameraRotateX
    public static VzExpression CameraRotateY
    public static VzExpression CameraTilt
    public static VzExpression CameraZoom
    //Part...
    public static VzPart FindPartByName(VzExpression partName)
}





public class VzTerrain : VizzyProxy
{
    public static VzExpression GetHeight(VzExpression location);
    public static VzExpression GetColor(VzExpression location);
}
public class VzPart : VizzyProxy
{
    public VzExpression ThisID;
    public VzExpression MaxID;
    public VzExpression MinID;
    public VzExpression PartName;
    public VzExpression Mass;
    public VzExpression Activated;
    public VzExpression PartType;
    public VzExpression Position;
    public VzExpression Temperature;
    public VzExpression Drag;
    public VzExpression UnderWater;
    public VzExpression Focused;
    public VzExpression Explode;
    public VzExpression LocalToPCI(VzExpression coords);
    public VzExpression PCIToLocal(VzExpression coords);
}



public abstract class VzWidget : VizzyProxy
{
    public VzInstruction EventMessage(WidgetEventType eventType, VzExpression msg, VzExpression data);
    public void Subscribe(WidgetEventType eventType, string msg, VzExpression data, System.Action<VzExpression> callback);
    public VzExpression AnchoredPosition;
    public VzExpression AnchorMin;
    public VzExpression AnchorMax;
    public VzExpression Color;
    public VzExpression Opacity;
    public VzExpression Parent;
    public VzExpression Pivot;
    public VzExpression Position;
    public VzExpression Rotation;
    public VzExpression Scale;
    public VzExpression Size;
    public VzExpression Visible;
}
public class VzLabel : VzWidget
{
    public VzExpression Text;
    public VzExpression FontSize;
    public VzExpression AutoSize;
    public Alignment Alignment;
}
public abstract class VzSprite : VzWidget
{
    public VzExpression FillMethod;
    public VzExpression Icon;
    public VzExpression FillAmount;
}
public class VzEllipse : VzSprite
{
}
public class VzRectangle : VzSprite
{
}
public class VzLine : VzSprite
{
    public VzExpression Thickness;
    public VzExpression Length;
}
public class VzRadialGauge : VzSprite
{
    public VzExpression BackgroundColor;
    public VzExpression FillColor;
    public VzExpression Text;
    public VzExpression TextColor;
    public VzExpression Value;
}
public class VzTexture : VzSprite
{\
    public void InitTexture(VzExpression width, VzExpression height);
    public void SetPixel(VzExpression x, VzExpression y, VzExpression color);
    public VzExpression GetPixel(VzExpression x, VzExpression y);
}
public class VzNavBall : VzSprite
{
    public VzExpression TopColor;
    public VzExpression BottomColor;
}
public class VzMap: VzSprite
{
    public VzExpression NorthUp;
    public VzExpression Zoom;
    public VzExpression ManualMode;
    public VzExpression PlanetName;
    public VzExpression Coordinates;
    public VzExpression Heading;
}
```  