using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Linq;

namespace REWVIZZY
{
    public enum WidgetType
    {
        Ellipse,
        Label,
        Line,
        RadialGauge,
        Rectangle,
        Texture,
        Navball,
        Map,
    }
    public enum WidgetAnchor
    {
        Left,
        Center,
        Right,
        TopLeft,
        TopCenter,
        TopRight,
        BottomLeft,
        BottomCenter,
        BottomRight,
    }

    /// <summary>
    /// 创建Widget
    /// </summary>
    public class CreateWidgetInstruction : VzInstruction
    {
        public WidgetType type;

        public VzExpression name;

        public CreateWidgetInstruction(WidgetType type, VzExpression name)
        {
            this.type = type;
            this.name = name;
        }

        public override XElement Serialize()
        {
            XElement xCreate = new XElement("SetCraftProperty",
                new XAttribute("property", "Mfd.Create." + type.ToString()),
                new XAttribute("id", this.id),
                new XAttribute("style", "create-mfd-widget")
                );

            xCreate.Add(name.Serialize());

            return xCreate;
        }
    }


    public enum WidgetPropertyType
    {
        AnchoredPosition,
        AnchorMin,
        AnchorMax,
        Color,
        Opacity,
        Parent,
        Pivot,
        Position,
        Rotation,
        Scale,
        Size,
        Visible,
    }

    /// <summary>
    /// 设置Widget属性
    /// </summary>
    public class SetWidgetInstruction : VzInstruction
    {
        public VzExpression name;

        public WidgetPropertyType propertySet;

        public VzExpression newvalue;

        public SetWidgetInstruction(VzExpression name, WidgetPropertyType setType, VzExpression value)
        {
            this.name = name;
            this.propertySet = setType;
            this.newvalue = value;
        }

        public override XElement Serialize()
        {
            XElement xSet = new XElement("SetCraftProperty",
                new XAttribute("property", "Mfd.Set" + this.propertySet.ToString()),
                new XAttribute("id", this.id),
                new XAttribute("style", "set-mfd-widget")
                );
            xSet.Add(name.Serialize());
            xSet.Add(newvalue.Serialize());
            return xSet;
        }
    }

    /// <summary>
    /// 设置锚点
    /// </summary>
    public class SetWidgetAnchorInstruction : VzInstruction
    {
        public VzExpression name;
        public WidgetAnchor anchor;

        public SetWidgetAnchorInstruction(VzExpression name, WidgetAnchor newAnchor)
        {
            this.name = name;
            this.anchor = newAnchor;
        }
        public override XElement Serialize()
        {
            XElement xSet = new XElement("SetCraftProperty",
                new XAttribute("property", "Mfd.Widget.SetAnchor." + anchor.ToString()),
                new XAttribute("id", this.id),
                new XAttribute("style", "set-mfd-anchor")
                );
            xSet.Add(name.Serialize());

            return xSet;
        }
    }

    public enum LabelProperyType
    {
        Text,
        FontSize,
        AutoSize,
    }
    /// <summary>
    /// 标签设置指令  
    /// </summary>
    public class SetLabelInstruction : VzInstruction
    {
        public LabelProperyType ptype;
        public VzExpression labelName;
        public VzExpression newValue;
        public SetLabelInstruction(VzExpression labelName, LabelProperyType property, VzExpression value)
        {
            this.labelName = labelName;
            this.ptype = property;
            this.newValue = value;
        }
        public override XElement Serialize()
        {
            XElement xSetLabel = new XElement("SetCraftProperty",
                new XAttribute("property", "Mfd.Label.Set" + ptype.ToString()),
                new XAttribute("id", this.id),
                new XAttribute("style", "set-mfd-label")
                ) ;
            xSetLabel.Add(labelName.Serialize());
            xSetLabel.Add(newValue.Serialize());
            return xSetLabel;
        }
    }





    public enum Alignment
    {
        Left,
        Center,
        Right,
        TopLeft,
        TopCenter,
        TopRight,
        BottomLeft,
        BottomCenter,
        BottomRight,
    }
    /// <summary>
    /// 设置字体对齐
    /// </summary>
    public class SetLabelAlignmentInstruction : VzInstruction
    {
        public VzExpression labelName;
        public Alignment alignment;
        public SetLabelAlignmentInstruction(VzExpression labelName, Alignment alignment)
        {
            this.labelName = labelName;
            this.alignment = alignment;
        }
        public override XElement Serialize()
        {
            XElement xSetAlignment = new XElement("SetCraftProperty",  
                new XAttribute("property", "Mfd.Label.SetAlignment." + alignment.ToString()),
                new XAttribute("id", this.id),
                new XAttribute("style", "set-mfd-alignment")
                );
            xSetAlignment.Add(labelName.Serialize());
            return xSetAlignment;
        }
    }

    /// <summary>
    /// 初始化纹理  
    /// </summary>
    public class InitTextureInstruction : VzInstruction
    {
        public VzExpression texName;
        public VzExpression width;
        public VzExpression height;

        public InitTextureInstruction (VzExpression textureName, VzExpression width, VzExpression height)
        {
            this.texName = textureName;
            this.width = width;
            this.height = height;
        }
        public override XElement Serialize()
        {
            XElement xInitTex = new XElement("SetCraftProperty",  
                new XAttribute("property", "Mfd.Texture.Initialize"),
                new XAttribute("id", this.id),
                new XAttribute("style", "set-mfd-texture-initialize")
                );
            xInitTex.Add(texName.Serialize());
            xInitTex.Add(width.Serialize());
            xInitTex.Add(height.Serialize());
            return xInitTex;
        }
    }

    /// <summary>
    /// 设置像素
    /// </summary>
    public class SetPixelInstruction: VzInstruction
    {
        public VzExpression texName;
        public VzExpression x;
        public VzExpression y;
        public VzExpression color;

        public SetPixelInstruction(VzExpression name, VzExpression x, VzExpression y, VzExpression color)
        {
            this.texName = name;
            this.x = x ;
            this.y = y;
            this.color = color;
        }
        public override XElement Serialize()
        {
            XElement xSetPixel = new XElement("SetCraftProperty",
                new XAttribute("property", "Mfd.Texture.SetPixel"),
                new XAttribute("id", this.id),
                new XAttribute("style", "set-mfd-texture-setpixel")
                );
            xSetPixel.Add(texName.Serialize());
            xSetPixel.Add(x.Serialize());
            xSetPixel.Add(y.Serialize());
            xSetPixel.Add(color.Serialize());
            return xSetPixel;
        }
    }

    public enum SpritePropertyType
    {
        FillMethod,
        Icon,
        FillAmount,
    }

    public class SetSpriteInstruction : VzInstruction
    {
        public VzExpression spriteName;
        public SpritePropertyType property;
        public VzExpression newValue;

        public SetSpriteInstruction(VzExpression spriteName, SpritePropertyType property, VzExpression value)
        {
            this.spriteName = spriteName;
            this.property = property;
            this.newValue = value;
        }

        public override XElement Serialize()
        {
            XElement xSetSprite = new XElement("SetCraftProperty",  
                new XAttribute("property", "Mfd.Sprite.Set" + property.ToString()),
                new XAttribute("id", this.id),
                new XAttribute("style", "set-mfd-sprite")
                );
            xSetSprite.Add(spriteName.Serialize());
            xSetSprite.Add(newValue.Serialize());
            return xSetSprite;
        }
    }

    public enum GaugePropertyType
    {
        BackgroundColor,
        FillColor,
        Text,
        TextColor,
        Value,
    }
    public class SetGaugeInstruction : VzInstruction
    {
        public VzExpression gaugeName;

        public GaugePropertyType property;

        public VzExpression newValue;

        public SetGaugeInstruction(VzExpression gaugeName, GaugePropertyType property, VzExpression newValue)
        {
            this.gaugeName = gaugeName;
            this.property = property;
            this.newValue = newValue;
        }
        public override XElement Serialize()
        {
            XElement xSetGauge = new XElement("SetCraftProperty",
                new XAttribute("property", "Mfd.Gauge.Set" + property.ToString()),
                new XAttribute("id", this.id),
                new XAttribute("style", "set-mfd-gauge")
                );
            xSetGauge.Add(gaugeName.Serialize());
            xSetGauge.Add(newValue.Serialize());
            return xSetGauge;
        }
    }


    public enum LinePropertyType
    {
        Thickness,
        Length,
    }

    /// <summary>
    /// 设置直线属性
    /// </summary>
    public class SetLineInstruction : VzInstruction
    {
        public VzExpression lineName;
        public LinePropertyType property;
        public VzExpression value;
        public SetLineInstruction(VzExpression lineName, LinePropertyType property, VzExpression value)
        {
            this.lineName = lineName;
            this.property = property;
            this.value = value;
        }
        public override XElement Serialize()
        {
            XElement xSetLine = new XElement("SetCraftProperty",
                new XAttribute("property", "Mfd.Line.Set" + property.ToString()),
                new XAttribute("id", this.id),
                new XAttribute("style", "set-mfd-line")
                );
            xSetLine.Add(lineName.Serialize());
            xSetLine.Add(value.Serialize());
            return xSetLine;
        }
    }

    /// <summary>
    /// 设置直线起始终点
    /// </summary>
    public class SetLinePointsInstructions : VzInstruction
    {
        public VzExpression lineName;
        public VzExpression pointA;
        public VzExpression pointB;
        public SetLinePointsInstructions(VzExpression lineName, VzExpression pointA, VzExpression pointB)
        {
            this.lineName = lineName;
            this.pointA= pointA;
            this.pointB= pointB;
        }
        public override XElement Serialize()
        {
            XElement xSetPoints = new XElement("SetCraftProperty",
                new XAttribute("property", "Mfd.Line.SetLinePoints"),
                new XAttribute("id", this.id),
                new XAttribute("style", "set-mfd-line-points")
                );
            xSetPoints.Add(lineName.Serialize());
            xSetPoints.Add(pointA.Serialize());
            xSetPoints.Add(pointB.Serialize());
            return xSetPoints;
        }
    }


    public enum NavBallPropertyType
    {
        TopColor,
        BottomColor,
    }
    public class SetNavBallInstruction : VzInstruction
    {
        public VzExpression name;
        public NavBallPropertyType property;
        public VzExpression value;
        public SetNavBallInstruction(VzExpression name, NavBallPropertyType property, VzExpression value)
        {
            this.name = name;
            this.property = property;
            this.value = value;
        }
        public override XElement Serialize()
        {
            XElement xSetNavBall = new XElement("SetCraftProperty",
                new XAttribute("property", "Mfd.Navball." + property.ToString()),
                new XAttribute("id", this.id),
                new XAttribute("style", "set-mfd-navball")
                );
            xSetNavBall.Add(name.Serialize());
            xSetNavBall.Add(value.Serialize());
            return xSetNavBall;
        }
    }


    public enum MapPropertyType
    {
        NorthUp,
        Zoom,
        ManualMode,
        PlanetName,
        Coordinates,
        Heading,
    }
    public class SetMapInstruction: VzInstruction
    {
        public VzExpression name;
        public MapPropertyType property;
        public VzExpression value;

        public SetMapInstruction(VzExpression name, MapPropertyType property, VzExpression value)
        {
            this.name = name;
            this.property = property;
            this.value = value;
        }

        public override XElement Serialize()
        {
            XElement xSetMap = new XElement("SetCraftProperty",
                new XAttribute("property", "Mfd.Map." + property.ToString()),
                new XAttribute("id", this.id),
                new XAttribute("style", "set-mfd-map")
                );
            xSetMap.Add(name.Serialize());
            xSetMap.Add(value.Serialize());
            return xSetMap;
        }
    }

    public class SendWidgeToFront : VzInstruction
    {
        public VzExpression name;
        public VzExpression target;
        public SendWidgeToFront (VzExpression name, VzExpression target )
        {
            this.name = name;
            this.target = target;
        }
        public override XElement Serialize()
        {
            XElement xFront = new XElement("SetCraftProperty",
                new XAttribute("property", "Mfd.Order.SendToFront"),
                new XAttribute("id", this.id),
                new XAttribute("style", "set-mfd-order-front")
                );
            xFront.Add(name.Serialize());
            xFront.Add(target.Serialize());
            return xFront;
        }
    }
    public class SendWidgeToBack : VzInstruction
    {
        public VzExpression name;
        public VzExpression target;
        public SendWidgeToBack(VzExpression name, VzExpression target)
        {
            this.name = name;
            this.target = target;
        }
        public override XElement Serialize()
        {
            XElement xBack = new XElement("SetCraftProperty",
                new XAttribute("property", "Mfd.Order.SendToBack"),
                new XAttribute("id", this.id),
                new XAttribute("style", "set-mfd-order-back")
                );
            xBack.Add(name.Serialize());
            xBack.Add(target.Serialize());
            return xBack;
        }
    }


    public enum WidgetEventType
    {
        PointerClick,
        PointerDown,
        PointerUp,
        Drag,
    }

    public class SetWidgetEventInstruction : VzInstruction
    {
        public VzExpression name;
        public WidgetEventType eventType;
        public VzExpression message;
        public VzExpression data;

        public SetWidgetEventInstruction(VzExpression name, WidgetEventType eventType, VzExpression message, VzExpression data)
        {
            this.name = name;
            this.eventType = eventType;
            this.message = message;
            this.data = data;
        }
        public override XElement Serialize()
        {
            XElement xWEvent = new XElement("SetCraftProperty",
                new XAttribute("property", "Mfd.Event.Set" + eventType.ToString()),
                new XAttribute("id", this.id),
                new XAttribute("style", "set-mfd-event")
                );
            xWEvent.Add(name.Serialize());
            xWEvent.Add(message.Serialize());
            xWEvent.Add(data.Serialize());
            return xWEvent;
        }
    }



    public class DestroyWidgetInstruction : VzInstruction
    {
        public VzExpression name;
        public DestroyWidgetInstruction(VzExpression name)
        {
            this.name = name;
        }
        public override XElement Serialize()
        {
            XElement xDestroy = new XElement("SetCraftProperty",
                new XAttribute("property", "Mfd.Destroy"),
                new XAttribute("id", this.id),
                new XAttribute("style", "destroy-mfd-widget")
                );
            xDestroy.Add(name.Serialize());
            return xDestroy;
        }
    }


    public class DestroyAllWidgetInstruction : VzInstruction
    {
        public override XElement Serialize()
        {
            XElement xDelAll = new XElement("SetCraftProperty",
                new XAttribute("property", "Mfd.Destroy.All"),
                new XAttribute("id", this.id),
                new XAttribute("style", "destroy-all-mfd-widgets")
                );
            return xDelAll;
        }
    }


    // **************************** Expressions ********************************

    public enum WidgetPropertyGetType
    {
        AnchoredPosition,
        AnchorMin,
        AnchorMax,
        Color,
        Exists,
        Opacity,
        Parent,
        Pivot,
        Position,
        Rotation,
        Scale,
        Size,
        Visible,
    }
    public class GetWidgePropertyExpression : VzExpression
    {
        public VzExpression name;
        public WidgetPropertyGetType propertyGet;

        public GetWidgePropertyExpression(VzExpression name, WidgetPropertyGetType propertyGet)
        {
            this.name = name;
            this.propertyGet = propertyGet;
        }

        public override XElement Serialize()
        {
            XElement xGet = new XElement("CraftProperty",
                new XAttribute("property", "Mfd." + propertyGet.ToString()),
                new XAttribute("style", "prop-mfd-widget")
                );
            xGet.Add(name.Serialize());
            return xGet;
        }
    }


    public class GetLabelPropertyExpression : VzExpression
    {
        public VzExpression name;
        public LabelProperyType property;
        public GetLabelPropertyExpression(VzExpression name, LabelProperyType property)
        {
            this.name = name;
            this.property = property;
        }
        public override XElement Serialize()
        {
            XElement xGetLabelProperty = new XElement("CraftProperty",
                new XAttribute("property", "Mfd.Label." + property.ToString()),
                new XAttribute("style", "prop-mfd-label")
                );
            xGetLabelProperty.Add(name.Serialize());
            return xGetLabelProperty;
        }
    }

    public class GetSpritePropertyExpression : VzExpression
    {
        public VzExpression name;
        public SpritePropertyType property;
        public GetSpritePropertyExpression(VzExpression name, SpritePropertyType property)
        {
            this.name = name;
            this.property = property;
        }
        public override XElement Serialize()
        {
            XElement xGetSpProp = new XElement("CraftProperty",
                new XAttribute("property", "Mfd.Sprite." + property.ToString()),
                new XAttribute("style", "prop-mfd-sprite")
                );
            xGetSpProp.Add(name.Serialize());
            return xGetSpProp;
        }
    }


    public class GetGaugePropertyExpression : VzExpression
    {
        public VzExpression name;

        public GaugePropertyType property;

        public GetGaugePropertyExpression(VzExpression name, GaugePropertyType property)
        {
            this.name = name;
            this.property = property;
        }
        public override XElement Serialize()
        {
            XElement xGet = new XElement("CraftProperty",
                new XAttribute("property", "Mfd.Gauge." + property.ToString()),
                new XAttribute("style", "prop-mfd-gauge")
                );
            xGet.Add(name.Serialize());
            return xGet;
        }
    }


    public class GetPixelExpression : VzExpression
    {
        public VzExpression name;
        public VzExpression x;
        public VzExpression y;
        public GetPixelExpression(VzExpression name, VzExpression x, VzExpression y)
        {
            this.name = name;
            this.x = x;
            this.y = y;
        }
        public override XElement Serialize()
        {
            XElement xGetPixel = new XElement("CraftProperty",
                new XAttribute("property", "Mfd.Texture.GetPixel"),
                new XAttribute("style", "prop-mfd-texture-getpixel")
                );
            xGetPixel.Add(name.Serialize());
            xGetPixel.Add(x.Serialize());
            xGetPixel.Add(y.Serialize());
            return xGetPixel;
        }
    }


    public class WidgetLocalToDisplayExpression : VzExpression
    {
        public VzExpression name;
        public VzExpression pos;
        public WidgetLocalToDisplayExpression (VzExpression name, VzExpression pos)
        {
            this.name = name;
            this.pos = pos;
        }
        public override XElement Serialize()
        {
            XElement xConvert = new XElement("CraftProperty",
                new XAttribute("property", "Mfd.LocaltoDisplay"),
                new XAttribute("style", "prop-mfd-pos")
                );
            xConvert.Add(name.Serialize());
            xConvert.Add(pos.Serialize());
            return xConvert;
        }
    }
    public class WidgetDisplayToLocalExpression : VzExpression
    {
        public VzExpression name;
        public VzExpression pos;
        public WidgetDisplayToLocalExpression(VzExpression name, VzExpression pos)
        {
            this.name = name;
            this.pos = pos;
        }
        public override XElement Serialize()
        {
            XElement xConvert = new XElement("CraftProperty",
                new XAttribute("property", "Mfd.DisplayToLocal"),
                new XAttribute("style", "prop-mfd-pos")
                );
            xConvert.Add(name.Serialize());
            xConvert.Add(pos.Serialize());
            return xConvert;
        }
    }


    public class GetWidgetEventMsgExpression : VzExpression
    {
        public VzExpression name;
        public WidgetEventType eventType;
        public GetWidgetEventMsgExpression(VzExpression name, WidgetEventType eventType)
        {
            this.name = name;
            this.eventType = eventType;
        }
        public override XElement Serialize()
        {
            XElement xEvtMsg = new XElement("CraftProperty",
                new XAttribute("property", "Mfd.Event." + eventType.ToString()),
                new XAttribute("style", "prop-mfd-event")
                );
            xEvtMsg.Add(name.Serialize());
            return xEvtMsg;
        }
    }


    public class HexColorExpression : VzExpression
    {
        public VzExpression text;
        public HexColorExpression (VzExpression text)
        {
            this.text = text;
        }
        public override XElement Serialize()
        {
            XElement xHex = new XElement("VectorOp",
                new XAttribute("op", "hex"),
                new XAttribute("style", "vec-op-color")
                );
            xHex.Add(text.Serialize());
            return xHex;
        }
    }
}
