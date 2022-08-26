


using System;

namespace vst
{
    public enum ETokenType 
    {
        SUM, SUB, OPEN, CLOSE, NUM, VAR, EOF
    }

    public class Token
    {
        public ETokenType Type {get;set;}
        public Int32 Value {get;set;}

        public Token(ETokenType type, Int32 value = 0)
        {
            Type = type;
            Value = value;
        }
    }

    public class Program
    {
        static string _input;
        static int _position = -1;
        static Token _lookahead;
        static Token NextToken()
        {
            char peek;
            _position++;
            if (_position < _input.Length)
                peek = _input[_position];
            else
                return new Token(ETokenType.EOF);
            switch (peek){
                case '+': return new Token(ETokenType.SUM);
                case '-': return new Token(ETokenType.SUB);
                case '(': return new Token(ETokenType.OPEN);
                case ')': return new Token(ETokenType.CLOSE);
            }
            if (Char.IsDigit(peek))
                return new Token(ETokenType.NUM);
            if (Char.IsLetter(peek))
                return new Token(ETokenType.VAR);
            
            Error("Erro Léxico");
            return new Token(ETokenType.EOF);
        }

        static void Error(string message)
        {           
            Console.WriteLine("********************************");
            Console.WriteLine("Erro! "+ message);
            Console.WriteLine(_input);
            Console.WriteLine("^".PadLeft(_position+1, ' '));
            Console.WriteLine("********************************");            
            Environment.Exit(0);
        }

        static void Log(string message)
        {           
            Console.WriteLine(">>>>"+ message);
        }

        static void Match(ETokenType type)
        {
            Log("match " + _lookahead.Type);
            if (_lookahead.Type == type)
                _lookahead = NextToken();
            else
                Error("Token inválido");
        }

        static void E() //E  ::= TR
        {
            Log("E " + _lookahead.Type);
            T();
            R();
        }
        static void R() //R  ::= + E | - E | ε
        {
            Log("R " + _lookahead.Type);
            if (_lookahead.Type == ETokenType.SUM)
            {
                Match(ETokenType.SUM);
                E();
            } 
            else if (_lookahead.Type == ETokenType.SUB)
            {
                Match(ETokenType.SUB);
                E();
            } 
            else if (_lookahead.Type != ETokenType.EOF)
            {
               Error("Símbolo inesperado em R");
            }
        }        
        static void T() //T  ::= ( E ) | NUM | VAR
        {
            Log("T " + _lookahead.Type);
            if (_lookahead.Type == ETokenType.OPEN)
            {       
                Match(ETokenType.OPEN);         
                E();
                Match(ETokenType.CLOSE);
            } 
            else if (_lookahead.Type == ETokenType.NUM)
            {
                Match(ETokenType.NUM);
                
            } 
            else if (_lookahead.Type == ETokenType.VAR)
            {
                Match(ETokenType.VAR);
            } else 
            {
                Error("Símbolo inesperado em T");
            }
        }


        public static void Main(string[] args)
        {
            Console.WriteLine("Insira a expressão");
            _input = Console.ReadLine();
            
            _lookahead = NextToken();
            E();
            Log("Sucesso");
        }
    }
}