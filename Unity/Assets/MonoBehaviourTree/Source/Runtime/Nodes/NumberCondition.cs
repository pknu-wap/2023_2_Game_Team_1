﻿using UnityEngine;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode(name = "Conditions/Number Condition")]
    public class NumberCondition : Condition
    {
        public Abort abort;
        public Type type = Type.Float;
        public FloatReference floatReference = new FloatReference(VarRefMode.DisableConstant);
        public IntReference intReference = new IntReference(VarRefMode.DisableConstant);
        public Comparator comparator = Comparator.Equal;
        public FloatReference floatReference2 = new FloatReference(0f);
        public IntReference intReference2 = new IntReference(0);

        // IMPROVEMENT: This class could be split into to different nodes
        protected override bool Check()
        {
            if (type == Type.Float)
            {
                switch (comparator)
                {
                    case Comparator.Equal:
                        return floatReference.Value == floatReference2.Value;
                    case Comparator.GreaterThan:
                        return floatReference.Value > floatReference2.Value;
                    case Comparator.LessThan:
                        return floatReference.Value < floatReference2.Value;
                }
            }
            else
            {
                switch (comparator)
                {
                    case Comparator.Equal:
                        return intReference.Value == intReference2.Value;
                    case Comparator.GreaterThan:
                        return intReference.Value > intReference2.Value;
                    case Comparator.LessThan:
                        return intReference.Value < intReference2.Value;
                }
            }
            return false;
        }

        public override void OnAllowInterrupt()
        {
            if (abort == Abort.None) return;
            ObtainTreeSnapshot();
            if (type == Type.Float) {
                floatReference.GetVariable().AddListener(OnVariableChange);
                if (!floatReference2.isConstant)
                {
                    floatReference2.GetVariable().AddListener(OnVariableChange);
                }
            } else {
                intReference.GetVariable().AddListener(OnVariableChange);
                if (!intReference2.isConstant)
                {
                    intReference2.GetVariable().AddListener(OnVariableChange);
                }
            }
        }

        public override void OnDisallowInterrupt()
        {
            if (abort == Abort.None) return;
            if (type == Type.Float) {
                floatReference.GetVariable().RemoveListener(OnVariableChange);
                if (!floatReference2.isConstant)
                {
                    floatReference2.GetVariable().RemoveListener(OnVariableChange);
                }
            } else {
                intReference.GetVariable().RemoveListener(OnVariableChange);
                if (!intReference2.isConstant)
                {
                    intReference2.GetVariable().RemoveListener(OnVariableChange);
                }
            }
        }

        private void OnVariableChange(float newVal, float oldVal)
        {
            EvaluateConditionAndTryAbort(abort);
        }

        private void OnVariableChange(int newVal, int oldVal)
        {
            EvaluateConditionAndTryAbort(abort);
        }

        public override bool IsValid()
        {
            return type switch
            {
                Type.Float => !(floatReference.isInvalid || floatReference2.isInvalid),
                Type.Int => !(intReference.isInvalid || intReference2.isInvalid),
                _ => true
            };
        }

        public enum Type
        {
            Float, Int
        }

        public enum Comparator
        {
            [InspectorName("==")]
            Equal, 
            [InspectorName(">")]
            GreaterThan, 
            [InspectorName("<")]
            LessThan
        }
    }
}
