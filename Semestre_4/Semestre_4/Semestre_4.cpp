


#include<iostream>
using namespace std; 

struct nodo 
{
	char dato; 
	nodo* sig = NULL;

};

class Cola 
{
private:
	nodo* frente; nodo* fin; 
public: 
	Cola();
	char desencolar();
	void encolar(char caracter); 
	bool cola_vacia();
	char ver_frente();

};

Cola::Cola() 
{
	frente = NULL;
	fin = NULL; 

}

bool Cola::cola_vacia()
{
	if (frente == NULL) 
	{
		return true;
	}
	else 
	{
		return false; 
	}

}

void Cola::encolar(char caracter)
{
	nodo* nuevo = new nodo();
	nuevo->dato = caracter;
	
	if (cola_vacia())
	{
		frente = nuevo;
		fin = nuevo;

	}
	else
	{
		fin->sig = nuevo;
		fin = nuevo; 
	}
				
}

char Cola::desencolar()
{
	
	if (!cola_vacia())
	{
		char caracter = frente->dato;
		nodo* aux = frente;
		if(fin->sig == NULL)
		{
			fin = NULL; 
			frente = NULL;
		}
		else 
		{
			frente = frente->sig;
		}
		delete aux;
		return caracter; 
		
	}
	else
	{
		cout << "cola vacia"; 

	}
	
}
char Cola::ver_frente()
{
	if (cola_vacia())
	{
		cout << "cola vaciaaaa";
	}
	else
	{
		return frente->dato;
	}
	
}



//CLASE PILA




class Pila 
{
private:
	nodo* top; nodo* base;
public:
	Pila();
	void push(char caracter);
	char pop();
	char ver_top();

};

Pila::Pila()
{
	top = NULL;
	base = NULL;
}

void Pila :: push(char caracter)
{
	nodo* nvo = new nodo();
	nvo->dato = caracter;
	if (top == NULL)
	{
		base = nvo;
		top = nvo;
		top->dato = nvo->dato;

	}
	else
	{
		top->sig = nvo ;
		top = nvo; 
	}
	


}

char Pila::pop() 
{
	
	
	if (top != NULL)
	{
		
		char caracter = top->dato;
		nodo* aux = top; 
		if (top->sig == NULL)
		{
			top = NULL;
			base = NULL; 
		}
		else
		{
			top = top->sig; 
		}
		delete aux;
		return caracter;

	}
	else
	{
		cout << "Pila Vacia"; 
		 
	}
}

char Pila::ver_top()
{
	if (base == NULL)
	{
		cout << "cola vacia chavalete" << endl;
		return 0; 
	}
	else
	{
		return top->dato;
	}
	
}

void menu_pila();
void menu_cola();
bool espalindromo(Cola& c, Pila& p); 



int main() 
{
	int op = 0, des = 0;
	char caracter;
	Pila pla; 
	Cola cla; 

	do
	{
		cout << "--MENU--" << endl;
		cout << "1- Ingresar palabra en pila" << endl;
		cout << "2- Ingresar palabra en cola" << endl;
		cout << "3- Comprobar Palindromo" << endl;
		cout << "4- Salir" << endl;
		cin >> des; 

		switch (des)
		{
		case 1:
			menu_pila();
			break;

		case 2: 
			menu_cola();
			break;

		case 3:
			espalindromo(cla, pla);
			if (espalindromo(cla, pla))
			{
				cout << "ES PALINDROMOOO <3";
				break;
			}
			else
			{
				cout << "No es Palindromo :(";
				break;
			}
			break;

		}


	} while (des != 4);
	
	
}




bool espalindromo(Cola& c, Pila& p)
{
	bool palindromo = true; 

	while (p.ver_top() != '\0' && c.cola_vacia())
	{
		char letra_p = p.pop();
		char letra_c = c.desencolar();
		if (letra_p != letra_c)
		{
			palindromo = false;
			break;

		}
	}

	while (p.ver_top() != '\0') p.pop();
	while (!c.cola_vacia()) c.desencolar();

	return palindromo; 
}

 
void menu_pila()
{
	int op = 0;
	char caracter;
	Pila p;
	do
	{
		cout << "MENU" << endl;
		cout << "1- ingresar letra" << endl;
		cout << "2- quitar letra" << endl;
		cout << "3- ver ultima letra ingresada" << endl;
		cin >> op;

		switch (op)
		{
		case 1:
			cout << "ingrese una letra: "; cin >> caracter;
			p.push(caracter);
			break;

		case 2:
			cout << "Letra eliminada: " << p.pop() << endl;
			break;

		case 3:
			cout << "El tope es: " << p.ver_top() << endl;
			break;
		}

	} while (op != 4);
}

void menu_cola()
{
	int op;
	char caracter; 
	Cola c;
	do
	{
		cout << "MENU" << endl;
		cout << "1- ingresar letra" << endl;
		cout << "2- quitar letra" << endl;
		cout << "3- ver ultima letra ingresada" << endl;
		cout << "4- Salir" << endl;
		cin >> op;

		switch (op)
		{
		case 1:
			cout << "ingrese una letra: "; cin >> caracter;
			c.encolar(caracter);
			break;

		case 2:
			cout << "Letra eliminada: " << c.desencolar();
			break;

		case 3:
			cout << "la primera letra es: " << c.ver_frente();
			break;
		}

	} while (op != 4);
}





