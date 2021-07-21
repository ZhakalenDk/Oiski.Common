using System;
using System.Collections.Generic;
using System.Text;

namespace Oiski.Common.Generics
{
    /// <summary>
    /// Defines a converter that specializes in the conversion and type casting of generic types
    /// </summary>
    public static class Converter
    {
        /// <summary>
        /// Cast the <see langword="value"/> of <typeparamref name="Type"/> to <typeparamref name="CType"/>
        /// </summary>
        /// <typeparam name="Type">The type of the <paramref name="_toCast"/></typeparam>
        /// <typeparam name="CType">The type to cast the <see langword="value"/> of <typeparamref name="Type"/> <paramref name="_toCast"/> to</typeparam>
        /// <param name="_toCast">The <see langword="value"/> to convert</param>
        /// <returns>The <see langword="value"/> of <paramref name="_toCast"/> if the conversion was successful; otherwise the <see langword="default"/> <see langword="value"/> of <paramref name="_toCast"/></returns>
        /// <exception cref="InvalidCastException"></exception>
        public static CType CastGeneric<Type, CType> (Type _toCast)
        {
            CType converted = default;

            try
            {
                if ( typeof(Type) == typeof(CType) )    //  If the type of two generics match
                {
                    converted = ( CType ) Convert.ChangeType(_toCast, typeof(CType));
                }
            }
            catch ( InvalidCastException _e )
            {

                throw new InvalidCastException("Invalid Conversion Arguments", _e);
            }

            return converted;
        }
    }
}
