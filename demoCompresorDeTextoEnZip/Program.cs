using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demoCompresorDeTextoEnZip
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {   //Se pueden generar texto de prueba en: 
                //http://www.blindtextgenerator.com/es

                //texto con 1000 caracteres de largo, se comprime a 354 caracteres
                String texto = "Una mañana, tras un sueño intranquilo, Gregorio Samsa se despertó convertido en un monstruoso insecto. Estaba echado de espaldas sobre un duro caparazón y, al alzar la cabeza, vio su vientre convexo y oscuro, surcado por curvadas callosidades, sobre el que casi no se aguantaba la colcha, que estaba a punto de escurrirse hasta el suelo. Numerosas patas, penosamente delgadas en comparación con el grosor normal de sus piernas, se agitaban sin concierto. - ¿Qué me ha ocurrido? No estaba soñando. Su habitación, una habitación normal, aunque muy pequeña, tenía el aspecto habitual. Sobre la mesa había desparramado un muestrario de paños - Samsa era viajante de comercio-, y de la pared colgaba una estampa recientemente recortada de una revista ilustrada y puesta en un marco dorado. La estampa mostraba a una mujer tocada con un gorro de pieles, envuelta en una estola también de pieles, y que, muy erguida, esgrimía un amplio manguito, asimismo de piel, que ocultaba todo su antebrazo. Gregorio mi";

                //texto con 1000 caracteres de largo, se comprime a 323 caracteres
                //String texto = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibus. Vivamus elementum semper nisi. Aenean vulputate eleifend tellus. Aenean leo ligula, porttitor eu, consequat vitae, eleifend ac, enim. Aliquam lorem ante, dapibus in, viverra quis, feugiat a, tellus. Phasellus viverra nulla ut metus varius laoreet. Quisque rutrum. Aenean imperdiet. Etiam ultricies nisi vel augue. Curabitur ullamcorper ultricies nisi. Nam eget dui. Etiam rhoncus. Maecenas tempus, tellus eget condimentum rhoncus, sem quam semper libero, sit amet adipiscing sem neque sed ipsum. N";

                /**
                NOTA:                
                Se crean dos textos uno con caracteres especiales y otro sin.

                Al realizar la compresion se debe convertir el string en byte[], pero se genera una caida de la libreria "Ionic.Zlib" al ejecutar la descompresion con:                
                --Encoding.ASCII.GetBytes(texto);

                Esto tiene que ver con el formato de codificacion de caracteres de la cadena de texto, se realizo la prueba con:
                --Encoding.Default.GetBytes(texto);
                La compresion y descompresion se realizo sin problemas, pero usa la codificacion por defecto del sistema operativo, 
                y si por algun motivo la codificacion para el idioma español no se encuentra (para uso de ej. ñ,ó,é, etc..),
                no deberia funcionar correctamente la descompresion o compresion.
                
                Para evitar los problemas anteriores, se insertaron las clases "BytesEncoder" y "StringEncoder" 
                que se encargan de realizar las conversiones de byte[] a string y string a byte[]


                Para un uso optimo, se debera buscar la correcta implementacion, 
                ya que al momento de realizar este proyecto demo, no se contaba con otros Windows en diferente idioma.

                Aunque la idea es comprimir archivos y no textos, para evitar problemas....

                **/
                Console.WriteLine("***Texto***");
                Console.WriteLine(texto);
                Console.WriteLine("=====================================");
                Console.WriteLine("Cantidad de caracteres del texto: " + texto.Length);
                Console.WriteLine();
                Console.WriteLine();
                //==========================================
                Console.WriteLine("***Texto Comprimido***");
                string textoComprimido = comprimirTexto(texto);
                Console.WriteLine(textoComprimido);
                Console.WriteLine("========================================");
                Console.WriteLine("Cantidad de caracteres del texto comprimido: " + textoComprimido.Length);
                Console.WriteLine();
                Console.WriteLine();
                //==========================================
                Console.WriteLine("***Texto Descomprimido***");
                string textoDescomprimido = descomprimirTexto(textoComprimido);
                Console.WriteLine(textoDescomprimido);
                Console.WriteLine("========================================");
                Console.WriteLine("Cantidad de caracteres del texto: " + textoDescomprimido.Length);
                Console.WriteLine("========================================");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                Console.ReadKey();
            }
        }
        /// <summary>
        /// Metodo encargado de comprimir un texto dado
        /// </summary>
        /// <param name="texto"></param>
        private static string comprimirTexto(string texto)
        {
            try
            {
                //Se convierte el texto a un array de byte
                byte[] bTextoSinComprimir = StringEncoder.EncodeToBytes(texto);
                //byte[] bTextoSinComprimir = Encoding.Default.GetBytes(texto);
                //byte[] bTextoSinComprimir = Encoding.ASCII.GetBytes(texto);
                //Se comprime el byte array
                byte[] bTextoComprimido = Ionic.Zlib.ZlibStream.CompressBuffer(bTextoSinComprimir);
                //Se convierte el byte[] a string para ser mostrado por la consola
                string textoComprimido = StringEncoder.DecodeToString(bTextoComprimido);
                //string textoComprimido = Encoding.Default.GetBytes(bTextoComprimido);
                //string textoComprimido = Encoding.ASCII.GetBytes(bTextoComprimido);
                return textoComprimido;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Metodo encargado de descomprimir un texto dado
        /// </summary>
        /// <param name="texto"></param>
        private static string descomprimirTexto(string texto)
        {
            try
            {
                //Se convierte el texto a un array de byte
                byte[] bTextoComprimido = StringEncoder.EncodeToBytes(texto);
                //byte[] bTextoComprimido = Encoding.Default.GetBytes(texto);
                //byte[] bTextoComprimido = Encoding.ASCII.GetBytes(texto);
                //Se descomprime el byte array
                byte[] bTextoSinComprimir = Ionic.Zlib.ZlibStream.UncompressBuffer(bTextoComprimido);
                //Se convierte el byte[] a string para ser mostrado por la consola, este sera el texto en "claro"
                string textoDescomprimido = StringEncoder.DecodeToString(bTextoSinComprimir);
                //string textoDescomprimido = Encoding.Default.GetBytes(bTextoSinComprimir);
                //string textoDescomprimido = Encoding.ASCII.GetBytes(bTextoSinComprimir);
                return textoDescomprimido;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
