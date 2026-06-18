using System;

namespace VideoPoker
{
    //Nodo
    public class Nodo<T>
    {
        public T Dato { get; set; }
        public Nodo<T> Siguiente { get; set; }

        public Nodo(T dato)
        {
            Dato = dato;
            Siguiente = null;
        }
    }

    //Lista enlazada 
    public class ListaEnlazada<T>
    {
        private Nodo<T> cabeza;
        private int contador;

        public Nodo<T> Cabeza { get { return cabeza; } }
        public int Contador { get { return contador; } }

        public ListaEnlazada()
        {
            cabeza = null;
            contador = 0;
        }

        public void Agregar(T dato)
        {
            Nodo<T> nuevo = new Nodo<T>(dato);

            if (cabeza == null)
            {
                cabeza = nuevo;
            }
            else
            {
                Nodo<T> actual = cabeza;
                while (actual.Siguiente != null)
                {
                    actual = actual.Siguiente;
                }
                actual.Siguiente = nuevo;
            }
            contador++;
        }

        public void Eliminar(T dato)
        {
            if (cabeza == null) return;

            if (cabeza.Dato.Equals(dato))
            {
                cabeza = cabeza.Siguiente;
                contador--;
                return;
            }

            Nodo<T> actual = cabeza;
            while (actual.Siguiente != null && !actual.Siguiente.Dato.Equals(dato))
            {
                actual = actual.Siguiente;
            }

            if (actual.Siguiente != null)
            {
                actual.Siguiente = actual.Siguiente.Siguiente;
                contador--;
            }
        }

        public T Buscar(Predicate<T> criterio)
        {
            Nodo<T> actual = cabeza;
            while (actual != null)
            {
                if (criterio(actual.Dato))
                    return actual.Dato;
                actual = actual.Siguiente;
            }
            return default(T);
        }

        public ListaEnlazada<T> Filtrar(Predicate<T> criterio)
        {
            ListaEnlazada<T> resultado = new ListaEnlazada<T>();
            Nodo<T> actual = cabeza;

            while (actual != null)
            {
                if (criterio(actual.Dato))
                    resultado.Agregar(actual.Dato);
                actual = actual.Siguiente;
            }

            return resultado;
        }

        public void Recorrer(Action<T> accion)
        {
            Nodo<T> actual = cabeza;
            while (actual != null)
            {
                accion(actual.Dato);
                actual = actual.Siguiente;
            }
        }

        public bool EstaVacia()
        {
            return cabeza == null;
        }

        public T ObtenerPorIndice(int indice)
        {
            if (indice < 0 || indice >= contador)
                throw new IndexOutOfRangeException("Índice fuera de rango");

            Nodo<T> actual = cabeza;
            for (int i = 0; i < indice; i++)
            {
                actual = actual.Siguiente;
            }

            return actual.Dato;
        }
    }

    //carta
    public class Carta
    {
        public string Valor { get; set; }
        public string Palo { get; set; }
        public int ValorNumerico { get; set; }

        public Carta(string valor, string palo)
        {
            Valor = valor;
            Palo = palo;
            ValorNumerico = ObtenerValorNumerico(valor);
        }

        private int ObtenerValorNumerico(string valor)
        {
            switch (valor)
            {
                case "A": return 1;
                case "J": return 11;
                case "Q": return 12;
                case "K": return 13;
                default:
                    if (int.TryParse(valor, out int num))
                        return num;
                    return 0;
            }
        }

        public override string ToString()
        {
            string simbolo = "";
            switch (Palo)
            {
                case "Corazones": simbolo = "♥"; break;
                case "Diamantes": simbolo = "♦"; break;
                case "Tréboles": simbolo = "♣"; break;
                case "Picas": simbolo = "♠"; break;
            }
            return $"{Valor}{simbolo}";
        }
    }

    //baraja
    public class Baraja
    {
        private ListaEnlazada<Carta> cartas;
        private Random random;

        public Baraja()
        {
            cartas = new ListaEnlazada<Carta>();
            random = new Random();
            CrearBaraja();
            Mezclar();
        }

        private void CrearBaraja()
        {
            string[] palos = { "Corazones", "Diamantes", "Tréboles", "Picas" };
            string[] valores = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

            foreach (string palo in palos)
            {
                foreach (string valor in valores)
                {
                    cartas.Agregar(new Carta(valor, palo));
                }
            }
        }

        public void Mezclar()
        {
            Carta[] arrayCartas = new Carta[cartas.Contador];

            Nodo<Carta> actual = cartas.Cabeza;
            for (int i = 0; i < arrayCartas.Length; i++)
            {
                arrayCartas[i] = actual.Dato;
                actual = actual.Siguiente;
            }

            for (int i = arrayCartas.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                Carta temp = arrayCartas[i];
                arrayCartas[i] = arrayCartas[j];
                arrayCartas[j] = temp;
            }

            cartas = new ListaEnlazada<Carta>();
            foreach (Carta carta in arrayCartas)
            {
                cartas.Agregar(carta);
            }
        }

        public Carta RobarCarta()
        {
            if (cartas.EstaVacia())
            {
                CrearBaraja();
                Mezclar();
            }

            Carta cartaRobada = cartas.ObtenerPorIndice(0);
            cartas.Eliminar(cartaRobada);
            return cartaRobada;
        }

        public int CartasRestantes()
        {
            return cartas.Contador;
        }
    }

    //Mano
    public class Mano
    {
        private ListaEnlazada<Carta> cartas;

        public Mano()
        {
            cartas = new ListaEnlazada<Carta>();
        }

        public void AgregarCarta(Carta carta)
        {
            cartas.Agregar(carta);
        }

        public void Mostrar()
        {
            Console.WriteLine("\n=== TU MANO ===");
            for (int i = 0; i < cartas.Contador; i++)
            {
                Console.WriteLine($"[{i + 1}] {cartas.ObtenerPorIndice(i)}");
            }
            Console.WriteLine();
        }

        //Método para obtener una carta específica
        public Carta ObtenerCarta(int indice)
        {
            if (indice < 0 || indice >= cartas.Contador)
                throw new ArgumentException("Índice de carta inválido");

            return cartas.ObtenerPorIndice(indice);
        }

        //Método para obtener cantidad de cartas
        public int CantidadCartas()
        {
            return cartas.Contador;
        }

        //Reemprazar carta
        public void ReemplazarCarta(int indice, Carta nuevaCarta)
        {
            if (indice < 0 || indice >= cartas.Contador)
                throw new ArgumentException("Índice inválido");

            //Crear nueva lista con la carta reemplazada
            ListaEnlazada<Carta> nuevaLista = new ListaEnlazada<Carta>();

            for (int i = 0; i < cartas.Contador; i++)
            {
                if (i == indice)
                {
                    nuevaLista.Agregar(nuevaCarta);
                }
                else
                {
                    nuevaLista.Agregar(cartas.ObtenerPorIndice(i));
                }
            }

            cartas = nuevaLista;
        }

        public string EvaluarMano()
        {
            if (EsEscaleraReal()) return "Escalera Real";
            if (EsEscaleraColor()) return "Escalera de Color";
            if (EsPoker()) return "Póker";
            if (EsFull()) return "Full";
            if (EsColor()) return "Color";
            if (EsEscalera()) return "Escalera";
            if (EsTrio()) return "Trio";
            if (EsDoblePar()) return "Doble Pareja";
            if (EsPar()) return "Un Par";

            return "Nada";
        }

        private bool EsPar()
        {
            int[] conteo = ContarValores();
            foreach (int count in conteo)
            {
                if (count == 2) return true;
            }
            return false;
        }

        private bool EsDoblePar()
        {
            int[] conteo = ContarValores();
            int pares = 0;
            foreach (int count in conteo)
            {
                if (count == 2) pares++;
            }
            return pares == 2;
        }

        private bool EsTrio()
        {
            int[] conteo = ContarValores();
            foreach (int count in conteo)
            {
                if (count == 3) return true;
            }
            return false;
        }

        private bool EsEscalera()
        {
            // Obtener valores de las cartas
            int[] valores = new int[cartas.Contador];
            for (int i = 0; i < cartas.Contador; i++)
            {
                valores[i] = cartas.ObtenerPorIndice(i).ValorNumerico;
            }

            Array.Sort(valores);

            // Verificar si son consecutivos
            for (int i = 0; i < valores.Length - 1; i++)
            {
                if (valores[i + 1] - valores[i] != 1)
                    return false;
            }

            return true;
        }

        private bool EsColor()
        {
            if (cartas.Contador == 0) return false;

            string primerPalo = cartas.ObtenerPorIndice(0).Palo;
            for (int i = 1; i < cartas.Contador; i++)
            {
                if (cartas.ObtenerPorIndice(i).Palo != primerPalo)
                    return false;
            }

            return true;
        }

        private bool EsFull()
        {
            int[] conteo = ContarValores();
            bool tieneTrio = false;
            bool tienePar = false;

            foreach (int count in conteo)
            {
                if (count == 3) tieneTrio = true;
                if (count == 2) tienePar = true;
            }

            return tieneTrio && tienePar;
        }

        private bool EsPoker()
        {
            int[] conteo = ContarValores();
            foreach (int count in conteo)
            {
                if (count == 4) return true;
            }
            return false;
        }

        private bool EsEscaleraColor()
        {
            return EsEscalera() && EsColor();
        }

        private bool EsEscaleraReal()
        {
            if (!EsColor()) return false;

            bool tiene10 = false, tieneJ = false, tieneQ = false, tieneK = false, tieneA = false;

            for (int i = 0; i < cartas.Contador; i++)
            {
                string valor = cartas.ObtenerPorIndice(i).Valor;
                if (valor == "10") tiene10 = true;
                if (valor == "J") tieneJ = true;
                if (valor == "Q") tieneQ = true;
                if (valor == "K") tieneK = true;
                if (valor == "A") tieneA = true;
            }

            return tiene10 && tieneJ && tieneQ && tieneK && tieneA;
        }

        private int[] ContarValores()
        {
            int[] conteo = new int[14]; // Índices 1-13 para valores de cartas

            for (int i = 0; i < cartas.Contador; i++)
            {
                int valor = cartas.ObtenerPorIndice(i).ValorNumerico;
                if (valor >= 1 && valor <= 13)
                {
                    conteo[valor]++;
                }
            }

            return conteo;
        }

        public decimal ObtenerValorPremio(string combinacion)
        {
            switch (combinacion)
            {
                case "Un Par": return 2;
                case "Doble Pareja": return 5;
                case "Trio": return 10;
                case "Escalera": return 20;
                case "Color": return 25;
                case "Full": return 30;
                case "Póker": return 50;
                case "Escalera de Color": return 100;
                case "Escalera Real": return 1000;
                default: return 0;
            }
        }
    }

    //Jugador 
    public class Jugador
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int CuponesGanados { get; set; }

        public Jugador(string cedula, string nombre, string apellido)
        {
            Cedula = cedula;
            Nombre = nombre;
            Apellido = apellido;
            CuponesGanados = 0;
        }

        public override string ToString()
        {
            return $"{Nombre} {Apellido} (Cédula: {Cedula}) - Cupones: {CuponesGanados}";
        }
    }

    // 7. CUPON
    public class Cupon
    {
        public string Codigo { get; set; }
        public string CedulaJugador { get; set; }
        public string NombreCompleto { get; set; }
        public decimal Valor { get; set; }
        public string Combinacion { get; set; }
        public DateTime Fecha { get; set; }

        public Cupon(string codigo, string cedula, string nombre, decimal valor, string combinacion)
        {
            Codigo = codigo;
            CedulaJugador = cedula;
            NombreCompleto = nombre;
            Valor = valor;
            Combinacion = combinacion;
            Fecha = DateTime.Now;
        }

        public override string ToString()
        {
            return $"Cupón #{Codigo} - {NombreCompleto} - {Combinacion} - ${Valor} - {Fecha:dd/MM/yyyy HH:mm}";
        }
    }

    // 8. PROGRAMA PRINCIPAL
    class Program
    {
        static ListaEnlazada<Jugador> jugadores = new ListaEnlazada<Jugador>();
        static ListaEnlazada<Cupon> cupones = new ListaEnlazada<Cupon>();
        static int contadorCupones = 0;

        static void Main(string[] args)
        {
            Console.Title = "VIDEO PÓKER - PIZZERÍA";
            Console.ForegroundColor = ConsoleColor.Cyan;

            bool salir = false;

            while (!salir)
            {
                Console.Clear();
                MostrarMenuPrincipal();

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        Jugar();
                        break;
                    case "2":
                        VerCupones();
                        break;
                    case "3":
                        VerTotalGastado();
                        break;
                    case "4":
                        VerTopJugador();
                        break;
                    case "5":
                        Console.WriteLine("\n¡Gracias por usar el sistema! ¡Vuelva pronto!");
                        salir = true;
                        break;
                    default:
                        Console.WriteLine("\nOpción inválida. Presione cualquier tecla...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void MostrarMenuPrincipal()
        {
            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("║     VIDEO PÓKER - PIZZERÍA          ║");
            Console.WriteLine("╠══════════════════════════════════════╣");
            Console.WriteLine("║ 1. Jugar                            ║");
            Console.WriteLine("║ 2. Ver cupones otorgados            ║");
            Console.WriteLine("║ 3. Ver total gastado en premios     ║");
            Console.WriteLine("║ 4. Ver jugador con más cupones      ║");
            Console.WriteLine("║ 5. Salir                            ║");
            Console.WriteLine("╚══════════════════════════════════════╝");
            Console.Write("\nSeleccione una opción: ");
        }

        static void Jugar()
        {
            Console.Clear();
            Console.WriteLine("=== NUEVO JUEGO ===\n");

            // Registrar o buscar jugador
            Jugador jugador = RegistrarJugador();
            if (jugador == null) return;

            // Iniciar juego
            Baraja baraja = new Baraja();
            Mano mano = new Mano();

            // Repartir 5 cartas iniciales
            Console.WriteLine("\nRepartiendo cartas...");
            for (int i = 0; i < 5; i++)
            {
                mano.AgregarCarta(baraja.RobarCarta());
            }

            mano.Mostrar();

            // Seleccionar cartas para conservar
            Console.WriteLine("¿Qué cartas desea conservar? (Ej: 1,3,5 o 0 para ninguna)");
            Console.Write("Ingrese los números separados por coma: ");
            string entrada = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(entrada) && entrada != "0")
            {
                string[] indicesStr = entrada.Split(',');
                bool[] conservar = new bool[5];

                foreach (string idxStr in indicesStr)
                {
                    if (int.TryParse(idxStr.Trim(), out int idx) && idx >= 1 && idx <= 5)
                    {
                        conservar[idx - 1] = true;
                    }
                }

                // Reemplazar cartas no conservadas
                for (int i = 0; i < 5; i++)
                {
                    if (!conservar[i])
                    {
                        mano.ReemplazarCarta(i, baraja.RobarCarta());
                    }
                }
            }

            Console.WriteLine("\n=== MANO FINAL ===");
            mano.Mostrar();

            // Evaluar mano
            string combinacion = mano.EvaluarMano();
            decimal premio = mano.ObtenerValorPremio(combinacion);

            if (premio > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n🎉 ¡FELICIDADES! Ganaste con: {combinacion}");
                Console.WriteLine($"💰 Premio: ${premio}");
                Console.ForegroundColor = ConsoleColor.Cyan;

                // Generar cupón
                string codigo = contadorCupones.ToString("D4");
                contadorCupones++;

                Cupon cupon = new Cupon(
                    codigo,
                    jugador.Cedula,
                    $"{jugador.Nombre} {jugador.Apellido}",
                    premio,
                    combinacion
                );

                cupones.Agregar(cupon);
                jugador.CuponesGanados++;

                Console.WriteLine($"\n📋 Cupón generado: #{codigo}");
                Console.WriteLine($"👤 Jugador: {jugador.Nombre} {jugador.Apellido}");
                Console.WriteLine($"💵 Valor: ${premio}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n😞 No obtuviste ninguna combinación ganadora");
                Console.WriteLine("¡Gracias por participar!");
                Console.ForegroundColor = ConsoleColor.Cyan;
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        static Jugador RegistrarJugador()
        {
            while (true)
            {
                Console.Write("Ingrese su cédula (solo números): ");
                string cedula = Console.ReadLine();

                // Validar que sean solo números
                bool esNumero = true;
                foreach (char c in cedula)
                {
                    if (!char.IsDigit(c))
                    {
                        esNumero = false;
                        break;
                    }
                }

                if (!esNumero || string.IsNullOrWhiteSpace(cedula))
                {
                    Console.WriteLine("Cédula inválida. Solo se permiten números.");
                    continue;
                }

                // Buscar jugador existente
                Jugador existente = jugadores.Buscar(j => j.Cedula == cedula);
                if (existente != null)
                {
                    Console.WriteLine($"\nBienvenido de nuevo, {existente.Nombre}!");
                    return existente;
                }

                // Datos nuevos
                Console.Write("Ingrese su nombre: ");
                string nombre = Console.ReadLine();

                Console.Write("Ingrese su apellido: ");
                string apellido = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellido))
                {
                    Console.WriteLine("Nombre y apellido son obligatorios.");
                    continue;
                }

                Jugador nuevo = new Jugador(cedula, nombre, apellido);
                jugadores.Agregar(nuevo);
                Console.WriteLine($"\n¡Bienvenido, {nombre}!");
                return nuevo;
            }
        }

        static void VerCupones()
        {
            Console.Clear();
            Console.WriteLine("=== CUPONES OTORGADOS ===\n");

            if (cupones.EstaVacia())
            {
                Console.WriteLine("No hay cupones registrados.");
            }
            else
            {
                cupones.Recorrer(cupon =>
                {
                    Console.WriteLine(cupon);
                });

                Console.WriteLine($"\nTotal de cupones: {cupones.Contador}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        static void VerTotalGastado()
        {
            Console.Clear();
            Console.WriteLine("=== TOTAL GASTADO EN PREMIOS ===\n");

            if (cupones.EstaVacia())
            {
                Console.WriteLine("No hay gastos registrados.");
            }
            else
            {
                decimal total = 0;
                Nodo<Cupon> actual = cupones.Cabeza;

                while (actual != null)
                {
                    total += actual.Dato.Valor;
                    actual = actual.Siguiente;
                }

                Console.WriteLine($"💵 Total gastado: ${total}");

                // Desglose por combinación
                Console.WriteLine("\n📊 Desglose por combinación:");
                string[] combinaciones = {
                    "Un Par", "Doble Pareja", "Trio", "Escalera",
                    "Color", "Full", "Póker", "Escalera de Color", "Escalera Real"
                };

                foreach (string combo in combinaciones)
                {
                    decimal subtotal = 0;
                    int cantidad = 0;

                    // Filtrar cupones por combinación
                    ListaEnlazada<Cupon> cuponesFiltrados = cupones.Filtrar(c => c.Combinacion == combo);

                    // Calcular subtotal
                    Nodo<Cupon> nodoActual = cuponesFiltrados.Cabeza;
                    while (nodoActual != null)
                    {
                        subtotal += nodoActual.Dato.Valor;
                        cantidad++;
                        nodoActual = nodoActual.Siguiente;
                    }

                    if (cantidad > 0)
                    {
                        Console.WriteLine($"  {combo}: {cantidad} cupones - ${subtotal}");
                    }
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        static void VerTopJugador()
        {
            Console.Clear();
            Console.WriteLine("=== JUGADOR CON MÁS CUPONES ===\n");

            if (jugadores.EstaVacia())
            {
                Console.WriteLine("No hay jugadores registrados.");
            }
            else
            {
                Jugador topJugador = null;
                int maxCupones = -1;

                Nodo<Jugador> actual = jugadores.Cabeza;
                while (actual != null)
                {
                    if (actual.Dato.CuponesGanados > maxCupones)
                    {
                        maxCupones = actual.Dato.CuponesGanados;
                        topJugador = actual.Dato;
                    }
                    actual = actual.Siguiente;
                }

                if (topJugador != null && maxCupones > 0)
                {
                    Console.WriteLine($"🏆 {topJugador.Nombre} {topJugador.Apellido}");
                    Console.WriteLine($"📋 Cédula: {topJugador.Cedula}");
                    Console.WriteLine($"🎫 Cupones ganados: {maxCupones}");

                    // Mostrar cupones de este jugador
                    Console.WriteLine("\n📜 Cupones obtenidos:");
                    ListaEnlazada<Cupon> cuponesJugador = cupones.Filtrar(c => c.CedulaJugador == topJugador.Cedula);

                    if (cuponesJugador.EstaVacia())
                    {
                        Console.WriteLine("  No tiene cupones registrados.");
                    }
                    else
                    {
                        cuponesJugador.Recorrer(c => Console.WriteLine($"  • {c.Combinacion} - ${c.Valor} ({c.Fecha:dd/MM/yyyy})"));
                    }
                }
                else
                {
                    Console.WriteLine("Ningún jugador ha ganado cupones aún.");
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}