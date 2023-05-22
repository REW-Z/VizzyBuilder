using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Linq;

namespace REWVIZZY
{
    public enum CraftInput
    {
        Roll,
        Pitch,
        Yaw,
        Throttle,
        Brake,
        Slider1,
        Slider2,
        Slider3,
        Slider4,
        TranslateForward,
        TranslateMode,
    }

    /// <summary>
    /// 激活阶段
    /// </summary>
    public class ActivateStageInstruction: VzInstruction
    {
        public override XElement Serialize()
        {
            return new XElement("ActivateStage", new XAttribute("id", this.id), new XAttribute("style", "activate-stage"));
        }
    }

    /// <summary>
    /// 设置输入  
    /// </summary>
    public class SetInputInstruction : VzInstruction
    {
        public CraftInput input;

        public VzExpression value;

        public SetInputInstruction(CraftInput input, VzExpression value)
        {
            this.input = input;
            this.value = value;
        }

        public override XElement Serialize()
        {
            XElement xSetInput = new XElement("SetInput",
                new XAttribute("input", this.input.ToString().ToLower()),
                new XAttribute("id", this.id),
                new XAttribute("style", "set-input")
                );
            xSetInput.Add(value.Serialize());

            return xSetInput;
        }
    }


    public enum TargetHeadingProperty
    {
        Heading,
        Pitch,
        Pid_Pitch,
        Pid_Roll,
    }
    /// <summary>
    /// TargetHeading
    /// </summary>
    public class SetTargetHeadingInstruction : VzInstruction
    {
        public TargetHeadingProperty property;
        public VzExpression value;

        public SetTargetHeadingInstruction(TargetHeadingProperty property, VzExpression value)
        {
            this.property = property;
            this.value = value;
        }

        public override XElement Serialize()
        {
            string propertyStr = "heading";
            string styleStr = "set-heading";
            switch (this.property)
            {
                case TargetHeadingProperty.Heading:
                    {
                        propertyStr = "heading";
                        styleStr = "set-heading";
                    }
                    break;
                case TargetHeadingProperty.Pitch:
                    {
                        propertyStr = "pitch";
                        styleStr = "set-heading";
                    }
                    break;
                case TargetHeadingProperty.Pid_Pitch:
                    {
                        propertyStr = "pid-pitch";
                        styleStr = "set-heading";
                    }
                    break;
                case TargetHeadingProperty.Pid_Roll:
                    {
                        propertyStr = "pid-roll";
                        styleStr = "set-heading";
                    }
                    break;
            }
            


            XElement xSetTargetHeading = new XElement("SetTargetHeading",
                new XAttribute("property", propertyStr),
                new XAttribute("id", this.id),
                new XAttribute("style", styleStr)
                );

            xSetTargetHeading.Add(value.Serialize());

            return xSetTargetHeading;
        }
    }

    /// <summary>
    /// 设置名为（name）的作品为 Target 目标
    /// </summary>
    public class SetTargetInstruction : VzInstruction
    {
        public VzExpression value;

        public SetTargetInstruction(VzExpression value)
        {
            this.value = value;
        }
        public override XElement Serialize()
        {
            XElement xInstruction = new XElement("SetTarget",
                new XAttribute("id", this.id),
                new XAttribute("style", "set-target")
                );

            xInstruction.Add(value.Serialize());
            return xInstruction;
        }
    }


    /// <summary>
    /// 开关AG
    /// </summary>
    public class SetActivateGroupInstruction : VzInstruction
    {
        public VzExpression ag;
        public VzExpression newvalue;

        public SetActivateGroupInstruction(VzExpression ag, VzExpression newvalue)
        {
            this.ag = ag;
            this.newvalue = newvalue;
        }

        public override XElement Serialize()
        {
            XElement xSetAg = new XElement("SetActivationGroup",
                new XAttribute("id", this.id),
                new XAttribute("style", "set-ag")
                );

            xSetAg.Add(ag.Serialize());
            xSetAg.Add(newvalue.Serialize());
            return xSetAg;
        }
    }

    public enum LockNavSphereIndicatorType
    {
        Vector,
        None,
        Prograde,
        Retrograde,
        Target,
        BurnNode,
        Current,
    }
    /// <summary>
    /// 此模块可以自动控制 pitch 和 yaw 以使你的作品的前方面对［］内所设置的目标。并不保证 roll 是否正常
    /// </summary>
    public class LockNavSphereInstruction : VzInstruction
    {
        public LockNavSphereIndicatorType type;
        public VzExpression valueForVector = null;

        public LockNavSphereInstruction(LockNavSphereIndicatorType type, VzExpression value = null)
        {
            this.type = type;
            if(type == LockNavSphereIndicatorType.Vector)
            {
                this.valueForVector = value;
            }
            else
            {
                this.valueForVector = null;
            }
        }


        public override XElement Serialize()
        {
            XAttribute xStyle = new XAttribute("style", "lock-nav-sphere");
            if (type == LockNavSphereIndicatorType.Vector) 
                xStyle = new XAttribute("style", "lock-nav-sphere-vector");

            XElement xLocNav = new XElement("LockNavSphere",
                new XAttribute("indicatorType", type.ToString()),
                new XAttribute("id", this.id),
                xStyle
                );

            if(type == LockNavSphereIndicatorType.Vector && !VzExpression.IsNull(valueForVector))
            {
                xLocNav.Add(valueForVector.Serialize());
            }

            return xLocNav;
        }
    }


    public enum TimeMode
    {
        Paused,
        SlowMotion,
        Normal,
        FastForward,
    }

    /// <summary>
    /// 设置时间模式
    /// </summary>
    public class SetTimeModeInstruction : VzInstruction
    {
        public TimeMode newMode;

        public SetTimeModeInstruction(TimeMode newnode)
        {
            this.newMode = newnode;
        }

        public override XElement Serialize()
        {
            return new XElement("SetTimeMode", 
                new XAttribute("mode", newMode.ToString()),
                new XAttribute("id", this.id.ToString()),
                new XAttribute("style", "set-time-mode")
                );
        }
    }


    public enum CameraProperty
    {
        rotateX,
        rotateY,
        tilt,
        zoom,
        mode,
        modeIndex,
    }
    /// <summary>
    /// 设置摄影机属性
    /// </summary>
    public class SetCameraPropertyInstruction : VzInstruction
    {
        public CameraProperty propertyToSet;

        public VzExpression value;

        public SetCameraPropertyInstruction(CameraProperty property, VzExpression newvalue)
        {
            this.propertyToSet = property;
            this.value = newvalue;
        }

        public override XElement Serialize()
        {
            XElement xSetCam = new XElement("SetCameraProperty",
                new XAttribute("property", this.propertyToSet.ToString()),
                new XAttribute("id", this.id),
                new XAttribute("style", "set-camera")
                );
            xSetCam.Add(value.Serialize());
            return xSetCam;
        }
    }


    public enum PartPropertySetType
    {
        Activated,
        Focused,
        Name,
        Explode,
    }
    /// <summary>
    /// 设置零件属性
    /// </summary>
    public class SetPartPropertyInstruction : VzInstruction
    {
        public VzExpression partId;

        public PartPropertySetType property;

        public VzExpression newValue;

        public SetPartPropertyInstruction(VzExpression partid, PartPropertySetType property, VzExpression newvalue)
        {
            this.partId = partid;
            this.property = property;
            this.newValue = newvalue;
        }


        public override XElement Serialize()
        {
            XElement xSet = new XElement("SetCraftProperty",
                new XAttribute("property", "Part.Set" + property.ToString()),
                new XAttribute("id", this.id),
                new XAttribute("style", "set-part")
                );

            xSet.Add(partId.Serialize());
            xSet.Add(newValue.Serialize());
            return xSet;
        }

    }

    /// <summary>
    /// 切换作品指令
    /// </summary>
    public class SwitchCraftInstruction : VzInstruction
    {
        public VzExpression craftID;

        public SwitchCraftInstruction(VzExpression craft)
        {
            this.craftID = craft;
        }

        public override XElement Serialize()
        {
            XElement xSwitch = new XElement("SwitchCraft",
                new XAttribute("id", this.id),
                new XAttribute("style", "switch-craft")
                );
            xSwitch.Add(craftID.Serialize());

            return xSwitch;
        }
    }

    /// <summary>
    /// 声音
    /// </summary>
    public class BeepInstruction: VzInstruction
    {
        public VzExpression frequency;
        public VzExpression volume;
        public VzExpression seconds;

        public BeepInstruction(VzExpression f, VzExpression v, VzExpression t)
        {
            this.frequency = f;
            this.volume = v;
            this.seconds = t; 
        }

        public override XElement Serialize()
        {
            XElement xBeep = new XElement("SetCraftProperty",
                new XAttribute("property", "Sound.Beep"),
                new XAttribute("id", this.id),
                new XAttribute("style", "play-beep")
                );
            xBeep.Add(frequency.Serialize());
            xBeep.Add(volume.Serialize());
            xBeep.Add(seconds.Serialize());
            return xBeep;
        }
    }
    


    // *********************************  Craft相关表达式 ****************************************

    public class CraftInputExpression : VzExpression
    {
        public CraftInput input;
        public CraftInputExpression(CraftInput inputGet)
        {
            this.input = inputGet;
        }
        public override XElement Serialize()
        {
            return new XElement("CraftProperty", new XAttribute("property", "Input." + input.ToString()), new XAttribute("style", "prop-input"));
        }
    }

    public enum CraftBasePropertyType
    {
        Altitude_AGL,
        Altitude_ASL,
        Altitude_ASF,
        Orbit_Apoapsis,
        Orbit_Periapsis,
        Orbit_TimeToApoapsis,
        Orbit_TimeToPeriapsis,
        Orbit_Eccentricity,
        Orbit_Inclination,
        Orbit_Period,
        Atmosphere_AirDensity,
        Atmosphere_AirPressure,
        Atmosphere_SpeedOfSound,
        Atmosphere_Temperature,
        Performance_CurrentEngineThrust,
        Performance_Mass,
        Performance_DryMass,
        Performance_FuelMass,
        Performance_MaxActiveEngineThrust,
        Performance_TWR,
        Performance_CurrentIsp,
        Performance_StageDeltaV,
        Performance_BurnTime,
        Fuel_Battery,
        Fuel_FuelInStage,
        Fuel_Mono,
        Fuel_AllStages,
        Nav_Position,
        Target_Position,
        Nav_CraftHeading,
        Nav_Pitch,
        Nav_BankAngle,
        Nav_AngleOfAttack,
        Nav_SideSlip,
        Nav_North,
        Nav_East,
        Nav_CraftDirection,
        Nav_CraftRight,
        Nav_CraftUp,
        Vel_SurfaceVelocity,
        Vel_OrbitVelocity,
        Target_Velocity,
        Vel_Gravity,
        Vel_Drag,
        Vel_Acceleration,
        Vel_AngularVelocity,
        Vel_LateralSurfaceVelocity,
        Vel_VerticalSurfaceVelocity,
        Vel_MachNumber,
        Misc_NumStages,
        Misc_Grounded,
        Misc_SolarRadiation,
        Misc_CameraPosition,
        Misc_CameraPointing,
        Misc_CameraUp,
        Misc_PidPitch,
        Misc_PidRoll,
        Time_FrameDeltaTime,
        Time_TimeSinceLaunch,
        Time_TotalTime,
        Time_WarpAmount,
        Name_Craft,
        Orbit_Planet,
        Target_Name,
        Target_Planet,
    }
    /// <summary>
    /// Craft基本属性
    /// </summary>
    public class CraftBaseProperty : VzExpression
    {
        public CraftBasePropertyType property;

        public CraftBaseProperty (CraftBasePropertyType type)
        {
            this.property = type;
        }

        public override XElement Serialize()
        {
            string styleStr = "";
            switch(this.property)
            {
                case CraftBasePropertyType.Altitude_AGL: 
                case CraftBasePropertyType.Altitude_ASL: 
                case CraftBasePropertyType.Altitude_ASF:
                    styleStr = "prop-altitude"; 
                    break;
                case CraftBasePropertyType.Orbit_Apoapsis:
                case CraftBasePropertyType.Orbit_Periapsis:
                case CraftBasePropertyType.Orbit_TimeToApoapsis:
                case CraftBasePropertyType.Orbit_TimeToPeriapsis:
                case CraftBasePropertyType.Orbit_Eccentricity:
                case CraftBasePropertyType.Orbit_Inclination:
                case CraftBasePropertyType.Orbit_Period:
                    styleStr = "prop-orbit";
                    break;
                case CraftBasePropertyType.Atmosphere_AirDensity:
                case CraftBasePropertyType.Atmosphere_AirPressure:
                case CraftBasePropertyType.Atmosphere_SpeedOfSound:
                case CraftBasePropertyType.Atmosphere_Temperature:
                    styleStr = "prop-atmosphere";
                    break;
                case CraftBasePropertyType.Performance_CurrentEngineThrust:
                case CraftBasePropertyType.Performance_Mass:
                case CraftBasePropertyType.Performance_DryMass:
                case CraftBasePropertyType.Performance_FuelMass:
                case CraftBasePropertyType.Performance_MaxActiveEngineThrust:
                case CraftBasePropertyType.Performance_TWR:
                case CraftBasePropertyType.Performance_CurrentIsp:
                case CraftBasePropertyType.Performance_StageDeltaV:
                case CraftBasePropertyType.Performance_BurnTime:
                    styleStr = "prop-performance";
                    break;
                case CraftBasePropertyType.Fuel_Battery:
                case CraftBasePropertyType.Fuel_FuelInStage:
                case CraftBasePropertyType.Fuel_Mono:
                case CraftBasePropertyType.Fuel_AllStages:
                    styleStr = "prop-fuel";
                    break;
                case CraftBasePropertyType.Nav_Position:
                case CraftBasePropertyType.Target_Position:
                case CraftBasePropertyType.Nav_CraftHeading:
                case CraftBasePropertyType.Nav_Pitch:
                case CraftBasePropertyType.Nav_BankAngle:
                case CraftBasePropertyType.Nav_AngleOfAttack:
                case CraftBasePropertyType.Nav_SideSlip:
                case CraftBasePropertyType.Nav_North:
                case CraftBasePropertyType.Nav_East:
                case CraftBasePropertyType.Nav_CraftDirection:
                case CraftBasePropertyType.Nav_CraftRight:
                case CraftBasePropertyType.Nav_CraftUp:
                    styleStr = "prop-nav";
                    break;
                case CraftBasePropertyType.Vel_SurfaceVelocity:
                case CraftBasePropertyType.Vel_OrbitVelocity:
                case CraftBasePropertyType.Target_Velocity:
                case CraftBasePropertyType.Vel_Gravity:
                case CraftBasePropertyType.Vel_Drag:
                case CraftBasePropertyType.Vel_Acceleration:
                case CraftBasePropertyType.Vel_AngularVelocity:
                case CraftBasePropertyType.Vel_LateralSurfaceVelocity:
                case CraftBasePropertyType.Vel_VerticalSurfaceVelocity:
                case CraftBasePropertyType.Vel_MachNumber:
                    styleStr = "prop-velocity";
                    break;
                case CraftBasePropertyType.Misc_NumStages:
                case CraftBasePropertyType.Misc_Grounded:
                case CraftBasePropertyType.Misc_SolarRadiation:
                case CraftBasePropertyType.Misc_CameraPosition:
                case CraftBasePropertyType.Misc_CameraPointing:
                case CraftBasePropertyType.Misc_CameraUp:
                case CraftBasePropertyType.Misc_PidPitch:
                case CraftBasePropertyType.Misc_PidRoll:
                    styleStr = "prop-misc";
                    break;
                case CraftBasePropertyType.Time_FrameDeltaTime:
                case CraftBasePropertyType.Time_TimeSinceLaunch:
                case CraftBasePropertyType.Time_TotalTime:
                case CraftBasePropertyType.Time_WarpAmount:
                    styleStr = "prop-time";
                    break;
                case CraftBasePropertyType.Name_Craft:
                case CraftBasePropertyType.Orbit_Planet:
                case CraftBasePropertyType.Target_Name:
                case CraftBasePropertyType.Target_Planet:
                    styleStr = "prop-name";
                    break;
                default:
                    break;
            }


            string propertyStr = property.ToString();
            propertyStr = propertyStr.Replace('_', '.');

            XElement xCraftProperty = new XElement("CraftProperty",
                new XAttribute("property", propertyStr),
                new XAttribute("style", styleStr)
                );
            return xCraftProperty;
        }

    }



    public class ActivationGroupExpression: VzExpression
    {
        private VzExpression agGet;

        public ActivationGroupExpression(VzExpression agGet)
        {
            this.agGet = agGet;
        }
        public override XElement Serialize()
        {
            XElement agExpr = new XElement("ActivationGroup",
                new XAttribute("style", "activation-group")
                );
            agExpr.Add(agGet.Serialize());

            return agExpr;
        }
    }





    /// <summary>
    /// POS - 经纬高转换
    /// </summary>
    public class PositionToLatLongAgl : VzExpression
    {
        public VzExpression pos;

        public PositionToLatLongAgl(VzExpression posVec)
        {
            this.pos = posVec;
        }

        public override XElement Serialize()
        {
            XElement xConvert = new XElement("Planet",
                new XAttribute("op", "toLatLongAgl"),
                new XAttribute("style", "planet-to-lat-long-agl")
                );

            xConvert.Add(pos.Serialize());
            return xConvert;
        }
    }
    /// <summary>
    /// 经纬高 - POS 转换
    /// </summary>
    public class LatLongAglToPosition : VzExpression
    {
        public VzExpression location;

        public LatLongAglToPosition(VzExpression vec)
        {
            this.location = vec;
        }
        public override XElement Serialize()
        {
            XElement xConvert = new XElement("Planet",
                new XAttribute("op", "toPosition"),
                new XAttribute("style", "planet-to-position")
                );
            xConvert.Add(location.Serialize());
            return xConvert;
        }
    }


    public enum TerrainPropertyType
    {
        Height,
        Color,
    }

    /// <summary>
    /// 地形查询
    /// </summary>
    public class TerrainPropertyExpression : VzExpression
    {
        public TerrainPropertyType type;
        public VzExpression location;

        public TerrainPropertyExpression(TerrainPropertyType type, VzExpression pos)
        {
            this.type = type;
            this.location = pos;
        }

        public override XElement Serialize()
        {
            XElement xQuery = new XElement("CraftProperty", 
                new XAttribute("property", "Terrain." + type.ToString()),
                new XAttribute("style", "terrain-query")
                );
            xQuery.Add(location.Serialize());
            return xQuery;
        }
    }


    public enum PlanetPropertyType
    {
        mass,
        radius,
        atmosphereHeight,
        soiradius,
        solarPosition,
        childPlanets,
        crafts,
        craftids,
        parent,
        structures,
        day,
        year,
        velocity,
        apoapsis,
        periapsis,
        period,
        apoapsistime,
        periapsistime,
        inclination,
        eccentricity,
        meananomaly,
        meanmotion,
        periapsisargument,
        rightascension,
        trueanomaly,
        semimajoraxis,
        semiminoraxis,
    }

    /// <summary>
    /// 行星属性
    /// </summary>
    public class PlanetPropertyExpression : VzExpression
    {
        public PlanetPropertyType propertyType;

        public VzExpression planet;

        public PlanetPropertyExpression(VzExpression planet, PlanetPropertyType type)
        {
            this.propertyType = type;
            this.planet = planet;
        }
        public override XElement Serialize()
        {
            XElement xPlanetP = new XElement("Planet",
                new XAttribute("op", propertyType.ToString()),
                new XAttribute("style", "planet")
                );
            xPlanetP.Add(planet.Serialize());
            return xPlanetP;
        }
    }


    public enum PartPropertyGetType
    {
        IDToName,
        Mass,
        Activated,
        PartType,
        Position,
        Temperature,
        Drag,
        ThisID,
        MinID,
        MaxID,
        UnderWater,
    }

    /// <summary>
    /// 获取零件属性
    /// </summary>
    public class PartProperty : VzExpression
    {
        public PartPropertyGetType propertyType;

        public VzExpression partId;

        public PartProperty(VzExpression part, PartPropertyGetType type)
        {
            this.propertyType = type;
            this.partId = part;
        }

        public override XElement Serialize()
        {
            XElement xPartProperty = new XElement("CraftProperty",
                new XAttribute("property", "Part." + propertyType.ToString()),
                new XAttribute("style", "part")
                );
            xPartProperty.Add(partId.Serialize());
            return xPartProperty;
        }
    }

    /// <summary>
    /// 零件Name TO ID
    /// </summary>
    public class PartNameToPartIDExpression : VzExpression
    {
        public VzExpression partName;
        public PartNameToPartIDExpression(VzExpression partName)
        {
            this.partName = partName;
        }
        public override XElement Serialize()
        {
            XElement xNameToId = new XElement("CraftProperty",
                new XAttribute("property", "Part.NameToID"),
                new XAttribute("style", "part-id")
                );
            xNameToId.Add(partName.Serialize());
            return xNameToId;
        }
    }

    /// <summary>
    /// 零件Local 2 Pci 
    /// </summary>
    public class PartLocalToPciExpression : VzExpression
    {
        public VzExpression partId;

        public VzExpression coords;

        public PartLocalToPciExpression(VzExpression partid, VzExpression coords)
        {
            this.partId = partid;
            this.coords = coords;
        }
        public override XElement Serialize()
        {
            XElement xConvert = new XElement("CraftProperty",
                new XAttribute("property", "Part.LocalToPci"),
                new XAttribute("style", "part-transform")
                );
            xConvert.Add(partId.Serialize());
            xConvert.Add(coords.Serialize());
            return xConvert;
        }
    }

    /// <summary>
    /// 零件 Pci  2 Local
    /// </summary>
    public class PartPciToLocalExpression : VzExpression
    {
        public VzExpression partId;

        public VzExpression coords;

        public PartPciToLocalExpression(VzExpression partid, VzExpression coords)
        {
            this.partId = partid;
            this.coords = coords;
        }
        public override XElement Serialize()
        {
            XElement xConvert = new XElement("CraftProperty",
                new XAttribute("property", "Part.PciToLocal"),
                new XAttribute("style", "part-transform")
                );
            xConvert.Add(partId.Serialize());
            xConvert.Add(coords.Serialize());
            return xConvert;
        }
    }




    public enum CraftOtherPropertyType
    {
        Altitude,
        Destroyed,
        Grounded,
        Mass,
        IDToName,
        PartCount,
        Planet,
        Position,
        Velocity,
        IsPlayer,
        Apoapsis,
        Periapsis,
        Period,
        ApoapsisTime,
        PeriapsisTime,
        Inclination,
        Eccentricity,
        MeanAnomaly,
        MeanMotion,
        PeriapsisArgument,
        RightAscension,
        TrueAnomaly,
        SemiMajorAxis,
        SemiMinorAxis,
    }

    /// <summary>
    /// 获取某飞机属性
    /// </summary>
    public class CraftOtherProperty : VzExpression
    {
        public VzExpression craftID;

        public CraftOtherPropertyType propertyType;

        public CraftOtherProperty(VzExpression craftID, CraftOtherPropertyType type)
        {
            this.craftID = craftID;
            this.propertyType = type;
        }

        public override XElement Serialize()
        {
            XElement xCraftP = new XElement("CraftProperty",
                new XAttribute("property", "Craft." + propertyType.ToString()),
                new XAttribute("style", "craft")
                );
            xCraftP.Add(craftID.Serialize());
            return xCraftP;
        }
    }
}
