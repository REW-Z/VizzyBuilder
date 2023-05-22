using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using REWJUNO;




namespace REWVIZZY
{
    public class VzField
    {
        public int offsetInOwner; //from 0
        public VzClass owner;

        public VzField(int dataIdx, VzClass fieldOwner)
        {
            this.offsetInOwner = dataIdx;
            this.owner = fieldOwner;
        }


        public VzExpression Value
        {
            //Get Set From 1
            get
            {
                if (!owner.isInited) owner.Init();
                return owner.data.Get(owner.offsetInData + this.offsetInOwner + 1); 
            } 
            set
            {
                if (!owner.isInited) owner.Init();
                owner.data.Set(owner.offsetInData + this.offsetInOwner + 1, value);
            }
        }
    }

    public class VzClass
    {
        //satic infos
        public static int instance_count = 0;
        public static List<VzClass> allVzInstances = new List<VzClass>();
        public static VzClass FindInstance(int instId)
        {
            return allVzInstances.FirstOrDefault(inst => inst.instanceID == instId);
        }
        public static void CheckInitialization()
        {
            foreach(var inst in allVzInstances)
            {
                if (!inst.isInited) inst.Init();
            }
        }



        //instance infos
        public int instanceID;
        public bool isInited = false;

        public int offsetInData = 0;
        public VzExpression data;


        public VzClass()
        {
            this.instanceID = instance_count;
            instance_count++;
            allVzInstances.Add(this);

            //auto init  
            Init();
        }

        public void Init(VzExpression initData = null)
        {
            if (isInited) return;
            if (Vz.context == null) return;
            if (Vz.context.instructionStack == null) return;
            if (Vz.context.instructionStack.Count < 1) return;
            if (Vz.context.instructionStack.Peek() == null) { return; }
            
            //Get All Instance Fields (Include Fields Of Members)
            List<string> debug = new List<string>();
            FieldInfo[] allFields = GetValidFields(this.GetType(), debug);

            //init data
            if (VzExpression.IsNull(initData))
            {
                this.data = Vz.DeclareListGlobal("vzclass+" + this.GetType().Name + "_instance_" + this.instanceID);
                using (var loop = new For(1, allFields.Length, 1))
                {
                    this.data.AddToList(0);
                }
            }
            //(LATER)set data  
            else
            {
                this.data = initData;
            }

            //Init Instance Fields  
            FieldInfo[] myInstanceFields = this.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            int idxInProxyList = 0;
            for (int i = 0; i < myInstanceFields.Length; ++i) // 1-length
            {
                if (myInstanceFields[i].FieldType == typeof(VzField))
                {
                    myInstanceFields[i].SetValue(this, new VzField(idxInProxyList, this)); //bug
                    idxInProxyList += 1;
                }
                else if (myInstanceFields[i].FieldType.IsSubclassOf(typeof(VzClass)))
                {
                    //myInstanceFields[i].SetValue()...     (LATER)：保存引用相关信息  
                    idxInProxyList += 1;
                }
                else if (IsStruct(myInstanceFields[i].FieldType)) //不需要
                {
                    object newStruct = Activator.CreateInstance(myInstanceFields[i].FieldType);
                    SetStuct(this, ref newStruct, ref idxInProxyList);
                    myInstanceFields[i].SetValue(this, newStruct);
                }
            }


            //Finished Initialize  
            isInited = true;
        }


        public static bool IsStruct(System.Type type)
        {
            if (type.IsValueType)
            {
                if (type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length > 1)
                {
                    return true;
                }
            }
            return false;
        }

        public static void SetStuct(VzClass finalOwner, ref object stucture, ref int idxInProxyList)
        {
            foreach (var f in stucture.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (f.FieldType == (typeof(VzField)))
                {
                    int idx = idxInProxyList;
                    f.SetValue(stucture, new VzField(idx, finalOwner));
                    idxInProxyList += 1;
                }
                else if (f.FieldType.IsSubclassOf(typeof(VzClass)))
                {
                    //...(LATER)：保存引用相关信息  
                    idxInProxyList += 1;
                }
                else if (IsStruct(f.FieldType)) //struct
                {
                    var fieldsOfStuct = GetValidFields(f.FieldType);
                    if (fieldsOfStuct.Length > 0)
                    {
                        object structSub = Activator.CreateInstance(f.FieldType);
                        SetStuct(finalOwner, ref structSub, ref idxInProxyList);
                        f.SetValue(stucture, structSub);
                    }
                }
            }
        }


        public static FieldInfo[] GetValidFields<T>() where T : VzClass
        {
            return VzClass.GetValidFields(typeof(T), null);
        }
        public static FieldInfo[] GetValidFields(System.Type type, List<string> debug = null)
        {
            if (debug != null)
            {
                debug.Add(type.Name);
                if (debug.Count > 99) return new FieldInfo[0];
            }



            List<FieldInfo> allFields = new List<FieldInfo>();

            foreach(var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if(field.FieldType == (typeof(VzField)))
                {
                    allFields.Add(field);
                }
                else if (field.FieldType.IsSubclassOf(typeof(VzClass)))
                {
                    allFields.Add(field);
                }
                else if (IsStruct(field.FieldType)) //struct
                {
                    var fieldsOfThis = GetValidFields(field.FieldType, debug);
                    if (fieldsOfThis.Length > 0)
                    {
                        allFields.AddRange(fieldsOfThis);
                    }
                }
            }

            return allFields.ToArray();
        }




        public static VzClass Convert<T>(VzExpression data) where T : VzClass
        {
            var obj = System.Activator.CreateInstance<T>();
            (obj as VzClass).Init(data);
            return obj;
        }
    }





    //public class ListFieldAttribute : System.Attribute
    //{
    //}

    //public class VzStack : VzClass
    //{
    //    //static 
    //    private static bool staticInited = false;
    //    private static VzAction CallPush;
    //    private static VzAction CallPop;
    //    private static VzFunc GetByIdx;


    //    //instance
    //    private bool isInited = false;

    //    public VzField stackTop;
    //    [ListField]
    //    public VzField data;



    //    public static void StaticInit()
    //    {
    //        CallPush = Vizzy.DeclareCustomInstruction("StackPush", "stackDataList", "stackTopVar", "newItem").SetInstructions((stackDataList, stackTopVar, newItem) => {
    //            stackTopVar.Value += 1;
    //            stackDataList.Set(stackTopVar.Value, newItem);
    //        });

    //        CallPop = Vizzy.DeclareCustomInstruction("StackPop", "stackDataList", "stackTopVar").SetInstructions((stackDataList, stackTopVar) => {
    //            stackTopVar.Value -= 1;
    //        });

    //        GetByIdx = Vizzy.DeclareCustomExpression("StackGet", "stackDataList", "idx").SetReturn((stackDataList, idx) => {
    //            return stackDataList.Get(idx.Value);
    //        });

    //        staticInited = true;
    //    }
    //    public void Init()
    //    {
    //        if (!staticInited) StaticInit();

    //        data.Value = Vizzy.DeclareListGlobal("List????");// new ListExpression(ListExpressionType.Create, ...);
    //        stackTop.Value = 1;

    //        using (var loop = new For(1, 99, 1))
    //        {
    //            data.Value.AddToList("");
    //        }
    //        isInited = true;
    //    }


    //    public void Push(VzExpression newItem)
    //    {
    //        if (!isInited) Init();

    //        CallPush(this.data.Value, this.stackTop.Value, newItem);
    //    }


    //    public VzExpression Pop()
    //    {
    //        if (!isInited) Init();

    //        CallPop(this.data.Value, this.stackTop.Value);
    //        return GetByIdx(this.data.Value, this.stackTop.Value + 1);
    //    }
    //}


}
