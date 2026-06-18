using System;
using System.Text.RegularExpressions;

namespace ProyectoListasManuales
{


    // 1. Módulo de Datos y Cálculos del Empleado
    public class Empleado
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cargo { get; set; }
        public double SalarioBase { get; set; }
        public int NumHijos { get; set; }
        public int HorasDiurnas { get; set; }
        public int HorasNocturnas { get; set; }
        public double SalarioTotal { get; private set; }

        public void CalcularSueldo()
        {
            // Fórmula: Base + (Diurnas * 1.20) + (Nocturnas * 2.00) + (Hijos * 10)
            this.SalarioTotal = SalarioBase + (HorasDiurnas * 1.20) + (HorasNocturnas * 2.00) + (NumHijos * 10);
        }

        // Método para clonar empleado (útil para reubicación)
        public Empleado Clonar()
        {
            return new Empleado
            {
                Cedula = this.Cedula,
                Nombre = this.Nombre,
                Apellido = this.Apellido,
                Cargo = this.Cargo,
                SalarioBase = this.SalarioBase,
                NumHijos = this.NumHijos,
                HorasDiurnas = this.HorasDiurnas,
                HorasNocturnas = this.HorasNocturnas
            };
        }
    }

    // 2. Estructuras de Listas Manuales
    public class NodoSimple
    {
        public Empleado Dato { get; set; }
        public NodoSimple Siguiente { get; set; }
        public NodoSimple(Empleado e) { Dato = e; }
    }

    public class ListaSimple
    {
        public NodoSimple Cabeza { get; private set; }

        // Agregar empleado
        public void Agregar(Empleado emp)
        {
            emp.CalcularSueldo();
            NodoSimple nuevo = new NodoSimple(emp);
            if (Cabeza == null) Cabeza = nuevo;
            else
            {
                NodoSimple actual = Cabeza;
                while (actual.Siguiente != null) actual = actual.Siguiente;
                actual.Siguiente = nuevo;
            }
        }

        // Eliminar empleado por cédula
        public bool Eliminar(string cedula)
        {
            if (Cabeza == null) return false;

            // Si el empleado a eliminar es la cabeza
            if (Cabeza.Dato.Cedula == cedula)
            {
                Cabeza = Cabeza.Siguiente;
                return true;
            }

            // Buscar empleado en la lista
            NodoSimple actual = Cabeza;
            while (actual.Siguiente != null && actual.Siguiente.Dato.Cedula != cedula)
            {
                actual = actual.Siguiente;
            }

            // Si se encontró el empleado
            if (actual.Siguiente != null && actual.Siguiente.Dato.Cedula == cedula)
            {
                actual.Siguiente = actual.Siguiente.Siguiente;
                return true;
            }

            return false;
        }

        // Buscar empleado por cédula
        public Empleado Buscar(string cedula)
        {
            NodoSimple actual = Cabeza;
            while (actual != null)
            {
                if (actual.Dato.Cedula == cedula) return actual.Dato;
                actual = actual.Siguiente;
            }
            return null;
        }

        // Modificar empleado
        public bool Modificar(string cedula, Empleado nuevosDatos)
        {
            Empleado empleado = Buscar(cedula);
            if (empleado == null) return false;

            // Actualizar datos del empleado
            empleado.Nombre = nuevosDatos.Nombre;
            empleado.Apellido = nuevosDatos.Apellido;
            empleado.Cargo = nuevosDatos.Cargo;
            empleado.SalarioBase = nuevosDatos.SalarioBase;
            empleado.NumHijos = nuevosDatos.NumHijos;
            empleado.HorasDiurnas = nuevosDatos.HorasDiurnas;
            empleado.HorasNocturnas = nuevosDatos.HorasNocturnas;

            // Recalcular sueldo
            empleado.CalcularSueldo();
            return true;
        }

        // Contar empleados
        public int Contar()
        {
            int contador = 0;
            NodoSimple actual = Cabeza;
            while (actual != null)
            {
                contador++;
                actual = actual.Siguiente;
            }
            return contador;
        }

        // Obtener empleado con mayor salario
        public Empleado ObtenerMayorSalario()
        {
            if (Cabeza == null) return null;

            Empleado mayor = Cabeza.Dato;
            NodoSimple actual = Cabeza.Siguiente;

            while (actual != null)
            {
                if (actual.Dato.SalarioTotal > mayor.SalarioTotal)
                {
                    mayor = actual.Dato;
                }
                actual = actual.Siguiente;
            }

            return mayor;
        }

        // Calcular suma total de salarios
        public double CalcularTotalSalarios()
        {
            double total = 0;
            NodoSimple actual = Cabeza;

            while (actual != null)
            {
                total += actual.Dato.SalarioTotal;
                actual = actual.Siguiente;
            }

            return total;
        }

        // Calcular promedio de horas diurnas
        public double CalcularPromedioHorasDiurnas()
        {
            if (Cabeza == null) return 0;

            double totalHoras = 0;
            int contador = 0;
            NodoSimple actual = Cabeza;

            while (actual != null)
            {
                totalHoras += actual.Dato.HorasDiurnas;
                contador++;
                actual = actual.Siguiente;
            }

            return totalHoras / contador;
        }
    }

    public class NodoDoble
    {
        public string Codigo { get; set; }
        public string NombreD { get; set; }
        public string Descripcion { get; set; }
        public ListaSimple Empleados { get; set; }
        public NodoDoble Siguiente { get; set; }
        public NodoDoble Anterior { get; set; }

        public NodoDoble(string cod, string nom, string desc)
        {
            Codigo = cod;
            NombreD = nom;
            Descripcion = desc;
            Empleados = new ListaSimple();
        }
    }

    public class ListaDoblePrincipal
    {
        public NodoDoble Cabeza { get; private set; }
        public NodoDoble Cola { get; private set; }

        
        public void acrearDepartamento(string cod, string nom, string desc)
        {
            if (!Regex.IsMatch(cod, @"^[A-Za-z]{3}[0-9]{3}$"))
            {
                Console.WriteLine("Error: Código debe tener formato LLLNNN (3 letras + 3 números)");
                return;
            }

            NodoDoble nuevo = new NodoDoble(cod, nom, desc);
            if (Cabeza == null) Cabeza = Cola = nuevo;
            else
            {
                Cola.Siguiente = nuevo;
                nuevo.Anterior = Cola;
                Cola = nuevo;
            }
            Console.WriteLine($"Departamento '{nom}' creado exitosamente.");
        }

        // Ver todos los departamentos y empleados
        public void ver()
        {
            if (Cabeza == null)
            {
                Console.WriteLine("No hay departamentos registrados.");
                return;
            }

            NodoDoble dep = Cabeza;
            while (dep != null)
            {
                Console.WriteLine($"\n>>> DPTO: {dep.NombreD} [{dep.Codigo}]");
                Console.WriteLine($"    {dep.Descripcion}");
                Console.WriteLine(new string('-', 75));
                Console.WriteLine("{0,-12} {1,-10} {2,-12} {3,-12} {4,-10}", "Cédula", "Nombre", "Apellido", "Cargo", "S. Total");

                NodoSimple emp = dep.Empleados.Cabeza;
                while (emp != null)
                {
                    Console.WriteLine("{0,-12} {1,-10} {2,-12} {3,-12} {4,-10:F2}",
                        emp.Dato.Cedula, emp.Dato.Nombre, emp.Dato.Apellido, emp.Dato.Cargo, emp.Dato.SalarioTotal);
                    emp = emp.Siguiente;
                }
                dep = dep.Siguiente;
            }
        }

        // Buscar departamento por código
        public NodoDoble Buscar(string cod)
        {
            NodoDoble act = Cabeza;
            while (act != null)
            {
                if (act.Codigo == cod) return act;
                act = act.Siguiente;
            }
            return null;
        }

        // Mostrar todos los códigos de departamentos disponibles
        public void MostrarDepartamentosDisponibles()
        {
            if (Cabeza == null)
            {
                Console.WriteLine("No hay departamentos registrados.");
                return;
            }

            Console.WriteLine("\n=== DEPARTAMENTOS DISPONIBLES ===");
            Console.WriteLine(new string('-', 50));
            Console.WriteLine("{0,-10} {1,-30} {2,-10}", "Código", "Nombre", "Empleados");
            Console.WriteLine(new string('-', 50));

            NodoDoble dep = Cabeza;
            while (dep != null)
            {
                Console.WriteLine("{0,-10} {1,-30} {2,-10}",
                    dep.Codigo, dep.NombreD, dep.Empleados.Contar());
                dep = dep.Siguiente;
            }
        }


        // 1. Calcular y mostrar total a pagar por departamento (ordenado de mayor a menor)
        public void MostrarTotalSalariosPorDepartamento()
        {
            if (Cabeza == null)
            {
                Console.WriteLine("No hay departamentos registrados.");
                return;
            }

            // Crear arreglo para ordenar
            int cantidad = 0;
            NodoDoble temp = Cabeza;
            while (temp != null)
            {
                cantidad++;
                temp = temp.Siguiente;
            }

            var departamentos = new NodoDoble[cantidad];
            var totales = new double[cantidad];

            // Llenar arreglos
            temp = Cabeza;
            for (int i = 0; i < cantidad; i++)
            {
                departamentos[i] = temp;
                totales[i] = temp.Empleados.CalcularTotalSalarios();
                temp = temp.Siguiente;
            }

            // Ordenar por burbuja (de mayor a menor)
            for (int i = 0; i < cantidad - 1; i++)
            {
                for (int j = 0; j < cantidad - i - 1; j++)
                {
                    if (totales[j] < totales[j + 1])
                    {
                        // Intercambiar departamentos
                        NodoDoble tempDep = departamentos[j];
                        departamentos[j] = departamentos[j + 1];
                        departamentos[j + 1] = tempDep;

                        // Intercambiar totales
                        double tempTotal = totales[j];
                        totales[j] = totales[j + 1];
                        totales[j + 1] = tempTotal;
                    }
                }
            }

            // Mostrar resultados
            Console.WriteLine("\n=== TOTAL A PAGAR POR DEPARTAMENTO (ORDENADO DE MAYOR A MENOR) ===");
            Console.WriteLine(new string('-', 70));
            Console.WriteLine("{0,-20} {1,-30} {2,15}", "Código", "Departamento", "Total a Pagar");
            Console.WriteLine(new string('-', 70));

            for (int i = 0; i < cantidad; i++)
            {
                Console.WriteLine("{0,-20} {1,-30} {2,15:F2}",
                    departamentos[i].Codigo,
                    departamentos[i].NombreD,
                    totales[i]);
            }
        }

        // 2. Calcular y mostrar promedio de salarios por departamento (ordenado de mayor a menor)
        public void MostrarPromedioSalariosPorDepartamento()
        {
            if (Cabeza == null)
            {
                Console.WriteLine("No hay departamentos registrados.");
                return;
            }

            // Crear arreglo para ordenar
            int cantidad = 0;
            NodoDoble temp = Cabeza;
            while (temp != null)
            {
                cantidad++;
                temp = temp.Siguiente;
            }

            var departamentos = new NodoDoble[cantidad];
            var promedios = new double[cantidad];

            // Llenar arreglos
            temp = Cabeza;
            for (int i = 0; i < cantidad; i++)
            {
                departamentos[i] = temp;
                double total = temp.Empleados.CalcularTotalSalarios();
                int empleados = temp.Empleados.Contar();
                promedios[i] = empleados > 0 ? total / empleados : 0;
                temp = temp.Siguiente;
            }

            // Ordenar por burbuja (de mayor a menor)
            for (int i = 0; i < cantidad - 1; i++)
            {
                for (int j = 0; j < cantidad - i - 1; j++)
                {
                    if (promedios[j] < promedios[j + 1])
                    {
                        // Intercambiar departamentos
                        NodoDoble tempDep = departamentos[j];
                        departamentos[j] = departamentos[j + 1];
                        departamentos[j + 1] = tempDep;

                        // Intercambiar promedios
                        double tempProm = promedios[j];
                        promedios[j] = promedios[j + 1];
                        promedios[j + 1] = tempProm;
                    }
                }
            }

            // Mostrar resultados
            Console.WriteLine("\n=== PROMEDIO DE SALARIOS POR DEPARTAMENTO (ORDENADO DE MAYOR A MENOR) ===");
            Console.WriteLine(new string('-', 70));
            Console.WriteLine("{0,-20} {1,-30} {2,15}", "Código", "Departamento", "Promedio Salario");
            Console.WriteLine(new string('-', 70));

            for (int i = 0; i < cantidad; i++)
            {
                Console.WriteLine("{0,-20} {1,-30} {2,15:F2}",
                    departamentos[i].Codigo,
                    departamentos[i].NombreD,
                    promedios[i]);
            }
        }

        // 3. Identificar y mostrar los empleados con los salarios más altos en cada departamento
        public void MostrarEmpleadosMayorSalarioPorDepartamento()
        {
            if (Cabeza == null)
            {
                Console.WriteLine("No hay departamentos registrados.");
                return;
            }

            Console.WriteLine("\n=== EMPLEADO CON MAYOR SALARIO POR DEPARTAMENTO ===");
            Console.WriteLine(new string('-', 85));
            Console.WriteLine("{0,-20} {1,-15} {2,-15} {3,-15} {4,15}",
                "Departamento", "Nombre", "Apellido", "Cargo", "Salario Total");
            Console.WriteLine(new string('-', 85));

            NodoDoble dep = Cabeza;
            while (dep != null)
            {
                Empleado mayor = dep.Empleados.ObtenerMayorSalario();
                if (mayor != null)
                {
                    Console.WriteLine("{0,-20} {1,-15} {2,-15} {3,-15} {4,15:F2}",
                        dep.NombreD, mayor.Nombre, mayor.Apellido, mayor.Cargo, mayor.SalarioTotal);
                }
                else
                {
                    Console.WriteLine("{0,-20} {1,64}", dep.NombreD, "Sin empleados");
                }
                dep = dep.Siguiente;
            }
        }

        // 4. Calcular y mostrar el promedio de horas diurnas trabajadas por departamento
        public void MostrarPromedioHorasDiurnasPorDepartamento()
        {
            if (Cabeza == null)
            {
                Console.WriteLine("No hay departamentos registrados.");
                return;
            }

            // Crear arreglo para ordenar
            int cantidad = 0;
            NodoDoble temp = Cabeza;
            while (temp != null)
            {
                cantidad++;
                temp = temp.Siguiente;
            }

            var departamentos = new NodoDoble[cantidad];
            var promedios = new double[cantidad];

            // Llenar arreglos
            temp = Cabeza;
            for (int i = 0; i < cantidad; i++)
            {
                departamentos[i] = temp;
                promedios[i] = temp.Empleados.CalcularPromedioHorasDiurnas();
                temp = temp.Siguiente;
            }

            // Ordenar por burbuja (de mayor a menor)
            for (int i = 0; i < cantidad - 1; i++)
            {
                for (int j = 0; j < cantidad - i - 1; j++)
                {
                    if (promedios[j] < promedios[j + 1])
                    {
                        // Intercambiar departamentos
                        NodoDoble tempDep = departamentos[j];
                        departamentos[j] = departamentos[j + 1];
                        departamentos[j + 1] = tempDep;

                        // Intercambiar promedios
                        double tempProm = promedios[j];
                        promedios[j] = promedios[j + 1];
                        promedios[j + 1] = tempProm;
                    }
                }
            }

            // Mostrar resultados
            Console.WriteLine("\n=== PROMEDIO DE HORAS DIURNAS POR DEPARTAMENTO (ORDENADO DE MAYOR A MENOR) ===");
            Console.WriteLine(new string('-', 70));
            Console.WriteLine("{0,-20} {1,-30} {2,15}", "Código", "Departamento", "Prom. Horas Diurnas");
            Console.WriteLine(new string('-', 70));

            for (int i = 0; i < cantidad; i++)
            {
                Console.WriteLine("{0,-20} {1,-30} {2,15:F2}",
                    departamentos[i].Codigo,
                    departamentos[i].NombreD,
                    promedios[i]);
            }
        }

        // 5. Reubicar empleado de un departamento a otro
        public bool ReubicarEmpleado(string cedula, string codigoOrigen, string codigoDestino)
        {
            // Validar existencia de departamentos
            NodoDoble depOrigen = Buscar(codigoOrigen);
            NodoDoble depDestino = Buscar(codigoDestino);

            if (depOrigen == null)
            {
                Console.WriteLine($"Error: Departamento origen '{codigoOrigen}' no encontrado.");
                return false;
            }

            if (depDestino == null)
            {
                Console.WriteLine($"Error: Departamento destino '{codigoDestino}' no encontrado.");
                return false;
            }

            // Buscar empleado en el departamento origen
            Empleado empleado = depOrigen.Empleados.Buscar(cedula);
            if (empleado == null)
            {
                Console.WriteLine($"Error: Empleado con cédula '{cedula}' no encontrado en {depOrigen.NombreD}.");
                return false;
            }

            // Clonar empleado
            Empleado empleadoClonado = empleado.Clonar();
            empleadoClonado.CalcularSueldo();

            // Agregar al departamento destino
            depDestino.Empleados.Agregar(empleadoClonado);

            // Eliminar del departamento origen
            bool eliminado = depOrigen.Empleados.Eliminar(cedula);

            if (eliminado)
            {
                Console.WriteLine($"Empleado {empleado.Nombre} {empleado.Apellido} reubicado exitosamente:");
                Console.WriteLine($"  De: {depOrigen.NombreD} [{codigoOrigen}]");
                Console.WriteLine($"  A:  {depDestino.NombreD} [{codigoDestino}]");
                return true;
            }

            return false;
        }

        // 6. Mostrar solo los jefes
        public void MostrarJefes()
        {
            Console.WriteLine("\n=== LISTA DE JEFES DE LA EMPRESA ===");
            Console.WriteLine(new string('-', 85));
            Console.WriteLine("{0,-12} {1,-10} {2,-12} {3,-20} {4,15}",
                "Cédula", "Nombre", "Apellido", "Departamento", "Salario Total");
            Console.WriteLine(new string('-', 85));

            NodoDoble dep = Cabeza;
            while (dep != null)
            {
                NodoSimple emp = dep.Empleados.Cabeza;
                while (emp != null)
                {
                    if (emp.Dato.Cargo == "Jefe")
                    {
                        Console.WriteLine("{0,-12} {1,-10} {2,-12} {3,-20} {4,15:F2}",
                            emp.Dato.Cedula, emp.Dato.Nombre, emp.Dato.Apellido,
                            dep.NombreD, emp.Dato.SalarioTotal);
                    }
                    emp = emp.Siguiente;
                }
                dep = dep.Siguiente;
            }
        }

        // 7. Filtrar empleados por departamento
        public void MostrarEmpleadosPorDepartamento(string codigo)
        {
            NodoDoble dep = Buscar(codigo);
            if (dep == null)
            {
                Console.WriteLine($"Error: Departamento con código '{codigo}' no encontrado.");
                return;
            }

            Console.WriteLine($"\n=== EMPLEADOS DEL DEPARTAMENTO: {dep.NombreD} [{dep.Codigo}] ===");
            Console.WriteLine($"Descripción: {dep.Descripcion}");
            Console.WriteLine(new string('-', 75));
            Console.WriteLine("{0,-12} {1,-10} {2,-12} {3,-12} {4,15}", "Cédula", "Nombre", "Apellido", "Cargo", "S. Total");
            Console.WriteLine(new string('-', 75));

            NodoSimple emp = dep.Empleados.Cabeza;
            double totalDepartamento = 0;

            while (emp != null)
            {
                Console.WriteLine("{0,-12} {1,-10} {2,-12} {3,-12} {4,15:F2}",
                    emp.Dato.Cedula, emp.Dato.Nombre, emp.Dato.Apellido,
                    emp.Dato.Cargo, emp.Dato.SalarioTotal);
                totalDepartamento += emp.Dato.SalarioTotal;
                emp = emp.Siguiente;
            }

            Console.WriteLine(new string('-', 75));
            Console.WriteLine("{0,65} {1,15:F2}", "TOTAL DEL DEPARTAMENTO:", totalDepartamento);
        }

        // 8. Modificar empleado
        public bool ModificarEmpleado(string cedula, Empleado nuevosDatos)
        {
            NodoDoble dep = Cabeza;
            while (dep != null)
            {
                if (dep.Empleados.Buscar(cedula) != null)
                {
                    return dep.Empleados.Modificar(cedula, nuevosDatos);
                }
                dep = dep.Siguiente;
            }
            return false;
        }

        // 9. Eliminar empleado
        public bool EliminarEmpleado(string cedula)
        {
            NodoDoble dep = Cabeza;
            while (dep != null)
            {
                if (dep.Empleados.Eliminar(cedula))
                {
                    Console.WriteLine($"Empleado con cédula '{cedula}' eliminado exitosamente del departamento {dep.NombreD}.");
                    return true;
                }
                dep = dep.Siguiente;
            }
            return false;
        }
    }

    class Program
    {
        // Constantes para validaciones
        private const int MAX_HORAS_DIURNAS = 8;
        private const int MAX_HORAS_NOCTURNAS = 8;
        private const int MAX_HIJOS = 5;
        private const int MAX_LONGITUD_CEDULA = 8;

        // Métodos de validación
        static bool ValidarSoloLetras(string texto)
        {
            return Regex.IsMatch(texto, @"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$");
        }

        static bool ValidarSoloNumeros(string texto)
        {
            return Regex.IsMatch(texto, @"^[0-9]+$");
        }

        static bool ValidarCedula(string cedula)
        {
            // Eliminar puntos si los tiene
            cedula = cedula.Replace(".", "");

            // Validar que sean solo números y longitud máxima
            if (!ValidarSoloNumeros(cedula) || cedula.Length > MAX_LONGITUD_CEDULA)
            {
                return false;
            }
            return true;
        }

        static string FormatearCedula(string cedula)
        {
            // Eliminar puntos si los tiene
            cedula = cedula.Replace(".", "");

            // Formatear con puntos
            if (cedula.Length >= 3)
            {
                return cedula.Insert(2, ".").Insert(6, ".");
            }
            return cedula;
        }

        static void Main(string[] args)
        {
            ListaDoblePrincipal sistema = new ListaDoblePrincipal();

            Console.WriteLine("=== SISTEMA DE GESTIÓN DE RECURSOS HUMANOS ===");
            Console.WriteLine("Cargando datos iniciales...\n");

            // ============================================
            // CARGA INICIAL DE DATOS (Parte 1)
            // ============================================

            // 1. VENTAS
            sistema.acrearDepartamento("Ven001", "Ventas", "Impulsar las estrategias comerciales y crecimiento.");
            var ven = sistema.Buscar("Ven001");
            ven.Empleados.Agregar(new Empleado { Cedula = "11.111.111", Nombre = "José", Apellido = "Gómez", Cargo = "Jefe", SalarioBase = 100, NumHijos = 2, HorasDiurnas = 8, HorasNocturnas = 1 });
            ven.Empleados.Agregar(new Empleado { Cedula = "22.222.222", Nombre = "María", Apellido = "López", Cargo = "Auxiliar", SalarioBase = 60, NumHijos = 0, HorasDiurnas = 8, HorasNocturnas = 0 });
            ven.Empleados.Agregar(new Empleado { Cedula = "33.444.555", Nombre = "Luis", Apellido = "Pérez", Cargo = "Gerente 2", SalarioBase = 80, NumHijos = 2, HorasDiurnas = 6, HorasNocturnas = 3 });

            // 2. INFORMÁTICA
            sistema.acrearDepartamento("Inf001", "Informática y sistemas", "Gestionar y mantener la infraestructura tecnológica.");
            var inf = sistema.Buscar("Inf001");
            inf.Empleados.Agregar(new Empleado { Cedula = "33.000.000", Nombre = "Carlos", Apellido = "Hernández", Cargo = "Jefe", SalarioBase = 200, NumHijos = 1, HorasDiurnas = 5, HorasNocturnas = 5 });
            inf.Empleados.Agregar(new Empleado { Cedula = "23.230.230", Nombre = "Luisa", Apellido = "Marcano", Cargo = "Gerente 1", SalarioBase = 100, NumHijos = 0, HorasDiurnas = 8, HorasNocturnas = 3 });
            inf.Empleados.Agregar(new Empleado { Cedula = "30.300.300", Nombre = "Antonio", Apellido = "Pérez", Cargo = "Gerente 2", SalarioBase = 80, NumHijos = 0, HorasDiurnas = 5, HorasNocturnas = 5 });

            // 3. PUBLICIDAD
            sistema.acrearDepartamento("Pub000", "Publicidad", "Planificar, dirigir, coordinar y ejecutar publicidad.");
            var pub = sistema.Buscar("Pub000");
            pub.Empleados.Agregar(new Empleado { Cedula = "12.000.000", Nombre = "Simón", Apellido = "Bolívar", Cargo = "Jefe", SalarioBase = 60, NumHijos = 1, HorasDiurnas = 10, HorasNocturnas = 5 });
            pub.Empleados.Agregar(new Empleado { Cedula = "10.000.000", Nombre = "José", Apellido = "González", Cargo = "Gerente 2", SalarioBase = 80, NumHijos = 2, HorasDiurnas = 8, HorasNocturnas = 0 });

            Console.WriteLine("Datos cargados exitosamente!\n");

            // ============================================
            // MENÚ PRINCIPAL (Integración de ambas partes)
            // ============================================

            bool salir = false;
            while (!salir)
            {
                Console.WriteLine("\n=== MENÚ PRINCIPAL ===");
                Console.WriteLine("1. Ver todos los empleados y departamentos");
                Console.WriteLine("2. Mostrar solo los jefes");
                Console.WriteLine("3. Filtrar empleados por departamento");
                Console.WriteLine("4. Análisis de salarios");
                Console.WriteLine("5. Análisis de horas trabajadas");
                Console.WriteLine("6. Reubicar empleado");
                Console.WriteLine("7. Agregar nuevo empleado");
                Console.WriteLine("8. Agregar nuevo departamento");
                Console.WriteLine("9. Modificar empleado");
                Console.WriteLine("10. Eliminar empleado");
                Console.WriteLine("11. Salir");
                Console.Write("\nSeleccione una opción: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1": // Ver todos
                        sistema.ver();
                        break;

                    case "2": // Mostrar jefes
                        sistema.MostrarJefes();
                        break;

                    case "3": // Filtrar por departamento
                        sistema.MostrarDepartamentosDisponibles();
                        Console.Write("\nIngrese código del departamento: ");
                        string codDepto = Console.ReadLine();
                        sistema.MostrarEmpleadosPorDepartamento(codDepto);
                        break;

                    case "4": // Análisis de salarios
                        Console.WriteLine("\n=== ANÁLISIS DE SALARIOS ===");
                        sistema.MostrarTotalSalariosPorDepartamento();
                        sistema.MostrarPromedioSalariosPorDepartamento();
                        sistema.MostrarEmpleadosMayorSalarioPorDepartamento();
                        break;

                    case "5": // Análisis de horas
                        Console.WriteLine("\n=== ANÁLISIS DE HORAS TRABAJADAS ===");
                        sistema.MostrarPromedioHorasDiurnasPorDepartamento();
                        break;

                    case "6": // Reubicar empleado
                        sistema.MostrarDepartamentosDisponibles();
                        Console.Write("\nCédula del empleado: ");
                        string cedula = Console.ReadLine();
                        Console.Write("Departamento origen (código): ");
                        string origen = Console.ReadLine();
                        Console.Write("Departamento destino (código): ");
                        string destino = Console.ReadLine();
                        sistema.ReubicarEmpleado(cedula, origen, destino);
                        break;

                    case "7": // Agregar nuevo empleado
                        try
                        {
                            Console.WriteLine("\n=== NUEVO EMPLEADO ===");

                            // Mostrar departamentos disponibles
                            sistema.MostrarDepartamentosDisponibles();

                            // Validar cédula
                            string ced;
                            while (true)
                            {
                                Console.Write("\nCédula (solo números, máximo 8 dígitos): ");
                                ced = Console.ReadLine();
                                if (ValidarCedula(ced))
                                {
                                    ced = FormatearCedula(ced);
                                    break;
                                }
                                Console.WriteLine("Error: Cédula inválida. Solo números y máximo 8 dígitos.");
                            }

                            // Validar nombre
                            string nom;
                            while (true)
                            {
                                Console.Write("Nombre (solo letras): ");
                                nom = Console.ReadLine();
                                if (ValidarSoloLetras(nom))
                                    break;
                                Console.WriteLine("Error: El nombre solo puede contener letras.");
                            }

                            // Validar apellido
                            string ape;
                            while (true)
                            {
                                Console.Write("Apellido (solo letras): ");
                                ape = Console.ReadLine();
                                if (ValidarSoloLetras(ape))
                                    break;
                                Console.WriteLine("Error: El apellido solo puede contener letras.");
                            }

                            // Validar cargo
                            string car;
                            while (true)
                            {
                                Console.Write("Cargo (Jefe, Gerente 1, Gerente 2, Auxiliar): ");
                                car = Console.ReadLine();
                                if (car == "Jefe" || car == "Gerente 1" || car == "Gerente 2" || car == "Auxiliar")
                                    break;
                                Console.WriteLine("Error: Cargo no válido. Opciones: Jefe, Gerente 1, Gerente 2, Auxiliar");
                            }

                            // Validar salario base
                            double salBase;
                            while (true)
                            {
                                Console.Write("Salario base: ");
                                if (double.TryParse(Console.ReadLine(), out salBase) && salBase >= 0)
                                    break;
                                Console.WriteLine("Error: Ingrese un número válido y positivo.");
                            }

                            // Validar número de hijos
                            int hijos;
                            while (true)
                            {
                                Console.Write($"Número de hijos (máximo {MAX_HIJOS}): ");
                                if (int.TryParse(Console.ReadLine(), out hijos) && hijos >= 0 && hijos <= MAX_HIJOS)
                                    break;
                                Console.WriteLine($"Error: Ingrese un número entre 0 y {MAX_HIJOS}.");
                            }

                            // Validar horas diurnas
                            int hrsD;
                            while (true)
                            {
                                Console.Write($"Horas diurnas (máximo {MAX_HORAS_DIURNAS}): ");
                                if (int.TryParse(Console.ReadLine(), out hrsD) && hrsD >= 0 && hrsD <= MAX_HORAS_DIURNAS)
                                    break;
                                Console.WriteLine($"Error: Ingrese un número entre 0 y {MAX_HORAS_DIURNAS}.");
                            }

                            // Validar horas nocturnas
                            int hrsN;
                            while (true)
                            {
                                Console.Write($"Horas nocturnas (máximo {MAX_HORAS_NOCTURNAS}): ");
                                if (int.TryParse(Console.ReadLine(), out hrsN) && hrsN >= 0 && hrsN <= MAX_HORAS_NOCTURNAS)
                                    break;
                                Console.WriteLine($"Error: Ingrese un número entre 0 y {MAX_HORAS_NOCTURNAS}.");
                            }

                            // Validar departamento
                            string codDep;
                            while (true)
                            {
                                Console.Write("Código departamento: ");
                                codDep = Console.ReadLine();
                                if (sistema.Buscar(codDep) != null)
                                    break;
                                Console.WriteLine("Error: Departamento no encontrado. Verifique el código.");
                                sistema.MostrarDepartamentosDisponibles();
                            }

                            NodoDoble departamento = sistema.Buscar(codDep);
                            if (departamento != null)
                            {
                                departamento.Empleados.Agregar(new Empleado
                                {
                                    Cedula = ced,
                                    Nombre = nom,
                                    Apellido = ape,
                                    Cargo = car,
                                    SalarioBase = salBase,
                                    NumHijos = hijos,
                                    HorasDiurnas = hrsD,
                                    HorasNocturnas = hrsN
                                });
                                Console.WriteLine("\n✓ Empleado agregado exitosamente.");
                            }
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Error: Ingrese valores válidos.");
                        }
                        break;

                    case "8": // Agregar nuevo departamento
                        Console.WriteLine("\n=== NUEVO DEPARTAMENTO ===");
                        Console.WriteLine("Formato del código: 3 letras + 3 números (ejemplo: RH001, CON002, FIN003)");

                        string cod;
                        while (true)
                        {
                            Console.Write("\nCódigo (formato LLLNNN): ");
                            cod = Console.ReadLine();
                            if (Regex.IsMatch(cod, @"^[A-Za-z]{3}[0-9]{3}$"))
                                break;
                            Console.WriteLine("Error: Formato inválido. Debe ser 3 letras seguidas de 3 números.");
                        }

                        string nombre;
                        while (true)
                        {
                            Console.Write("Nombre (solo letras y espacios): ");
                            nombre = Console.ReadLine();
                            if (ValidarSoloLetras(nombre))
                                break;
                            Console.WriteLine("Error: El nombre solo puede contener letras y espacios.");
                        }

                        string desc;
                        while (true)
                        {
                            Console.Write("Descripción: ");
                            desc = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(desc))
                                break;
                            Console.WriteLine("Error: La descripción no puede estar vacía.");
                        }

                        sistema.acrearDepartamento(cod, nombre, desc);
                        break;

                    case "9": // Modificar empleado
                        Console.WriteLine("\n=== MODIFICAR EMPLEADO ===");
                        Console.Write("Ingrese la cédula del empleado a modificar: ");
                        string cedulaMod = Console.ReadLine();

                        // Buscar empleado
                        NodoDoble depBuscar = sistema.Cabeza;
                        Empleado empModificar = null;
                        string deptoEmpleado = "";

                        while (depBuscar != null && empModificar == null)
                        {
                            empModificar = depBuscar.Empleados.Buscar(cedulaMod);
                            if (empModificar != null)
                            {
                                deptoEmpleado = depBuscar.NombreD;
                                break;
                            }
                            depBuscar = depBuscar.Siguiente;
                        }

                        if (empModificar == null)
                        {
                            Console.WriteLine($"Error: No se encontró empleado con cédula {cedulaMod}");
                            break;
                        }

                        Console.WriteLine($"\nEmpleado encontrado: {empModificar.Nombre} {empModificar.Apellido}");
                        Console.WriteLine($"Departamento: {deptoEmpleado}");
                        Console.WriteLine("\nIngrese los nuevos datos (deje en blanco para mantener el valor actual):");

                        // Nuevos datos
                        string nuevoNombre;
                        while (true)
                        {
                            Console.Write($"Nombre [{empModificar.Nombre}]: ");
                            nuevoNombre = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nuevoNombre))
                            {
                                nuevoNombre = empModificar.Nombre;
                                break;
                            }
                            if (ValidarSoloLetras(nuevoNombre))
                                break;
                            Console.WriteLine("Error: El nombre solo puede contener letras.");
                        }

                        string nuevoApellido;
                        while (true)
                        {
                            Console.Write($"Apellido [{empModificar.Apellido}]: ");
                            nuevoApellido = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nuevoApellido))
                            {
                                nuevoApellido = empModificar.Apellido;
                                break;
                            }
                            if (ValidarSoloLetras(nuevoApellido))
                                break;
                            Console.WriteLine("Error: El apellido solo puede contener letras.");
                        }

                        string nuevoCargo;
                        while (true)
                        {
                            Console.Write($"Cargo [{empModificar.Cargo}]: ");
                            nuevoCargo = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nuevoCargo))
                            {
                                nuevoCargo = empModificar.Cargo;
                                break;
                            }
                            if (nuevoCargo == "Jefe" || nuevoCargo == "Gerente 1" || nuevoCargo == "Gerente 2" || nuevoCargo == "Auxiliar")
                                break;
                            Console.WriteLine("Error: Cargo no válido. Opciones: Jefe, Gerente 1, Gerente 2, Auxiliar");
                        }

                        double nuevoSalarioBase;
                        while (true)
                        {
                            Console.Write($"Salario base [{empModificar.SalarioBase}]: ");
                            string inputSalario = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(inputSalario))
                            {
                                nuevoSalarioBase = empModificar.SalarioBase;
                                break;
                            }
                            if (double.TryParse(inputSalario, out nuevoSalarioBase) && nuevoSalarioBase >= 0)
                                break;
                            Console.WriteLine("Error: Ingrese un número válido y positivo.");
                        }

                        int nuevosHijos;
                        while (true)
                        {
                            Console.Write($"Número de hijos [{empModificar.NumHijos}]: ");
                            string inputHijos = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(inputHijos))
                            {
                                nuevosHijos = empModificar.NumHijos;
                                break;
                            }
                            if (int.TryParse(inputHijos, out nuevosHijos) && nuevosHijos >= 0 && nuevosHijos <= MAX_HIJOS)
                                break;
                            Console.WriteLine($"Error: Ingrese un número entre 0 y {MAX_HIJOS}.");
                        }

                        int nuevasHorasD;
                        while (true)
                        {
                            Console.Write($"Horas diurnas [{empModificar.HorasDiurnas}]: ");
                            string inputHorasD = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(inputHorasD))
                            {
                                nuevasHorasD = empModificar.HorasDiurnas;
                                break;
                            }
                            if (int.TryParse(inputHorasD, out nuevasHorasD) && nuevasHorasD >= 0 && nuevasHorasD <= MAX_HORAS_DIURNAS)
                                break;
                            Console.WriteLine($"Error: Ingrese un número entre 0 y {MAX_HORAS_DIURNAS}.");
                        }

                        int nuevasHorasN;
                        while (true)
                        {
                            Console.Write($"Horas nocturnas [{empModificar.HorasNocturnas}]: ");
                            string inputHorasN = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(inputHorasN))
                            {
                                nuevasHorasN = empModificar.HorasNocturnas;
                                break;
                            }
                            if (int.TryParse(inputHorasN, out nuevasHorasN) && nuevasHorasN >= 0 && nuevasHorasN <= MAX_HORAS_NOCTURNAS)
                                break;
                            Console.WriteLine($"Error: Ingrese un número entre 0 y {MAX_HORAS_NOCTURNAS}.");
                        }

                        // Crear objeto con nuevos datos
                        Empleado datosActualizados = new Empleado
                        {
                            Cedula = cedulaMod,
                            Nombre = nuevoNombre,
                            Apellido = nuevoApellido,
                            Cargo = nuevoCargo,
                            SalarioBase = nuevoSalarioBase,
                            NumHijos = nuevosHijos,
                            HorasDiurnas = nuevasHorasD,
                            HorasNocturnas = nuevasHorasN
                        };

                        // Aplicar modificación
                        if (sistema.ModificarEmpleado(cedulaMod, datosActualizados))
                        {
                            Console.WriteLine("\n✓ Empleado modificado exitosamente.");
                        }
                        else
                        {
                            Console.WriteLine("\n✗ Error al modificar el empleado.");
                        }
                        break;

                    case "10": // Eliminar empleado
                        Console.WriteLine("\n=== ELIMINAR EMPLEADO ===");
                        Console.Write("Ingrese la cédula del empleado a eliminar: ");
                        string cedulaEliminar = Console.ReadLine();

                        if (sistema.EliminarEmpleado(cedulaEliminar))
                        {
                            Console.WriteLine("\n✓ Empleado eliminado exitosamente.");
                        }
                        else
                        {
                            Console.WriteLine($"\n✗ Error: No se encontró empleado con cédula {cedulaEliminar}");
                        }
                        break;

                    case "11": // Salir
                        salir = true;
                        Console.WriteLine("Saliendo del sistema...");
                        break;

                    default:
                        Console.WriteLine("Opción no válida. Intente de nuevo.");
                        break;
                }

                if (!salir)
                {
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
    }
}