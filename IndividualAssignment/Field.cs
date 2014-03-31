using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndividualAssignment
{
    public abstract class Field
    {
        public double GetDistance(Field other)
        {
            double returnVal = -1;
            if (GetType() == typeof(NumericField))
            {
                returnVal = Compare(other);
            }
            if (GetType() == typeof(NominalField))
            {
                returnVal = Compare(other);
            }
            return returnVal;
        }

        protected abstract double Compare(Field other);
    }

    public class NumericField : Field
    {
        public double NumericValue { get; private set; }

        public NumericField(double numericValue)
        {
            NumericValue = numericValue;
        }

        protected override double Compare(Field field)
        {
            if(field.GetType() != typeof(NumericField)) throw new Exception("Wrong type");
            return Math.Abs(NumericValue - ((NumericField) field).NumericValue);
        } 
    }

    public class NominalField : Field
    {
        public int NominalValue { get; private set; }

        public NominalField(int nominalValue)
        {
            NominalValue = nominalValue;
        }

        protected override double Compare(Field field)
        {
            if (field.GetType() != typeof(NominalField)) throw new Exception("Wrong type");
            int otherNomVal = ((NominalField) field).NominalValue;
            return otherNomVal != NominalValue ? 1.0 : 0.0;
        }
    }
}
