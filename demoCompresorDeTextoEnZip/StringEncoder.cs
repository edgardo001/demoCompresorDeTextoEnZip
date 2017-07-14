using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demoCompresorDeTextoEnZip
{
    /// <summary>
    /// Nota en: https://stackoverflow.com/questions/16072709/converting-string-to-byte-array-in-c-sharp
    /// A refinement to JustinStolle's edit (Eran Yogev's use of BlockCopy).
    /// The proposed solution is indeed faster than using Encoding. Problem is that it doesn't work for encoding byte arrays of uneven length. As given, it raises an out-of-bound exception. Increasing the length by 1 leaves a trailing byte when decoding from string.
    /// For me, the need came when I wanted to encode from DataTable to JSON.I was looking for a way to encode binary fields into strings and decode from string back to byte[].
    /// I therefore created two classes - one that wraps the above solution(when encoding from strings it's fine, because the lengths are always even), and another that handles byte[] encoding. I solved the uneven length problem by adding a single character that tells me if the original length of the binary array was odd ('1') or even ('0')
    ///
    /// Traduccion Español:
    /// Un refinamiento a la edición de JustinStolle (uso de BlockCopy de Eran Yogev).
    /// La solución propuesta es más rápida que la codificación.El problema es que no funciona para codificar arrays de bytes de longitud irregular.Como se da, se plantea una excepción fuera de línea.Aumentar la longitud por 1 deja un byte de arrastre al decodificar desde una cadena.
    /// Para mí, la necesidad llegó cuando quería codificar desde DataTable a JSON. Buscaba una forma de codificar campos binarios en cadenas y decodificar de cadena de nuevo a byte[].
    /// Por lo tanto, creo dos clases - una que envuelve la solución anterior (cuando la codificación de las cadenas está bien, porque las longitudes son siempre iguales), y otro que maneja byte[] de codificación.Resolví el problema de longitud irregular agregando un solo carácter que me dice si la longitud original de la matriz binaria era impar('1') o incluso('0')
    /// </summary>
    public static class StringEncoder
    {
        public static byte[] EncodeToBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
        public static string DecodeToString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
    }
}
