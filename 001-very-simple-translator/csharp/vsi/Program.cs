using System;

namespace vsi
{
    public enum ETokenType 
    {
        SUM, SUB, OPEN, CLOSE, NUM, EOF
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
                return new Token(ETokenType.NUM, Int32.Parse(peek.ToString()));
           
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

        static int E() //E  ::= TR
        {
            Log("E " + _lookahead.Type);
            var res1 = T();
            var res2 = R(res1);
            return res2;
        }
        static int R(int t) //R  ::= + E | - E | ε
        {
            Log("R " + _lookahead.Type);
            if (_lookahead.Type == ETokenType.SUM)  //FIRST
            {
                Match(ETokenType.SUM);
                var res = E();
                return t + res;
            } 
            else if (_lookahead.Type == ETokenType.SUB) //FIRST
            {
                Match(ETokenType.SUB);
                var res = E();
                return t - res;
            } 
            else if ((_lookahead.Type != ETokenType.EOF) && (_lookahead.Type != ETokenType.CLOSE))//FOLLOW
            {
               Error("Símbolo inesperado em R");
            }
            return t;
        }        
        static int T() //T  ::= ( E ) | NUM | VAR
        {
            Log("T " + _lookahead.Type);
            if (_lookahead.Type == ETokenType.OPEN)
            {       
                Match(ETokenType.OPEN);         
                var res = E();
                Match(ETokenType.CLOSE);
                return res;
            } 
            else if (_lookahead.Type == ETokenType.NUM)
            {
                var res = _lookahead.Value;
                Match(ETokenType.NUM);
                return res;
            } 
            else  
            {
                Error("Símbolo inesperado em T");
                
            }
            return 0;
        }


        public static void Main(string[] args)
        {
            Console.WriteLine("Insira a expressão");
            _input = Console.ReadLine();
            _lookahead = NextToken();
            var res = E();
            Log("Sucesso");
            Log("O resultado é: "+ res);
        }
    }
}