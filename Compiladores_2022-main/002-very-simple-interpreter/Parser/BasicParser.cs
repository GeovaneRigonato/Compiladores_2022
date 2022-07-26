using VerySimpleInterpreter.Lexer;

namespace VerySimpleInterpreter.Parser
{
    public class BasicParser
    {
        private BasicLexer _lexer;
        private Token? _lookAhead;
        public BasicParser(BasicLexer lexer)
        {
            _lexer = lexer;
        }

        public void Match(ETokenType type)
        {
            if (_lookAhead?.Type == type)
                _lookAhead = _lexer.GetNextToken();
            else
                Error("Expected " + type + " -found" + _lookAhead?.Type);
        }

        public void Error(String msg)
        {
            Console.WriteLine("_________________________________________");
            Console.WriteLine("Erro na linha " + _lexer.Line);
            Console.WriteLine("_________________________________________");
            Console.WriteLine(msg);
            Console.WriteLine("_________________________________________");
        }

        /*
in     : INPUT VAR
out    : OUTPUT VAR
atrib  : VAR AT expr
expr   : termY
Y      : vazio | + expr | - expr
term   : factZ
Z      : vazio | * term | / term
fact   : NUM | VAR | OE expr CE
        */

        /*
        prog   : lineX
        X      : EOF | prog
        line   : stmt EOL
        stmt   : in | out | atrib
        in     : INPUT VAR
        out    : OUTPUT VAR
        atrib  : VAR AT expr
        expr   : termY
        Y      : vazio | + expr | - expr
        term   : factZ
        Z      : vazio | * term | / term
        fact   : NUM | VAR | OE expr CE
        */

        public void Prog() // prog   : lineX
        {
            Line();
            X();
        }

        public void X() //X : EOF | prog
        {
            if (_lookAhead?.Type == ETokenType.EOF)
                Match(ETokenType.EOF);
            else
                Prog();
        }

        public void Line() // line   : stmt EOL
        {
            Stmt();
            Match(ETokenType.EOL);
        }
    
        public void Stmt() //stmt   : in | out | atrib  
        {
             if (_lookAhead?.Type == ETokenType.INPUT)
                 Input();
             else if (_lookAhead?.Type == ETokenType.OUTPUT)
                 Output();
             else if (_lookAhead?.Type == ETokenType.VAR)
                 Atrib();
             else
                Error("Expected " + type + " -found" + _lookAhead?.Type);
        }
        public void Input() //input     : INPUT VAR
        {
                Match(ETokenType.INPUT);
                Match(ETokenType.VAR);
        }
        
        public void Output() //output    : OUTPUT VAR
        {
                Match(ETokenType.OUTPUT);
                Match(ETokenType.VAR);
        }
        public void Atrib()//atrib  : VAR AT expr
        {
                Match(ETokenType.VAR);
                Match(ETokenType.AT);
                Expr();
        }
        public void Expr()//expr   : termY
        {
            Term();
            Y();
        }

        public void Term()//term   : factZ
        {
          // var f = Fact();
         //  return Z(f);
        }
        public void Y()//Y      : vazio | + expr | - expr
        {
            if (_lookAhead?.Type == ETokenType.SUM)
            {
                Match(ETokenType.SUM);
                Expr();
            }
                
            else if (_lookAhead?.Type == ETokenType.SUB)
            {
                Match(ETokenType.SUB);
                Expr();
            }
            else if (!TestFollow(ETokenType.CE,ETokenType.EOL)){  //CE ou EOL
                Error("Expected SUM, SUB, CE OU EOL");
            }
            else
                Error("Expected " + type + " -found" + _lookAhead?.Type);

        }
        public void Fact()//fact   : NUM | VAR | OE expr CE
        {
            if (_lookAhead?.Type == ETokenType.NUM)
                Match(ETokenType.NUM);
            else if (_lookAhead?.Type == ETokenType.VAR)
                Match(ETokenType.VAR);
            else if (_lookAhead?.Type == ETokenType.OE)
            {
                Match(ETokenType.OE);
                Expr();
                if (_lookAhead?.Type == ETokenType.CE)
                Match(ETokenType.CE);
                else
                    Error("Expected " + type + " -found" + _lookAhead?.Type);
            }
            else
                Error("Expected " + type + " -found" + _lookAhead?.Type);
                

        }

        public void Z(){ //vazio | * term | / term
            if (_lookAhead?.Type == ETokenType.MULT)
            {
                Match(ETokenType.MULT);
                Term();
            }
            else if (_lookAhead?.Type == ETokenType.DIV)
            {
                Match(ETokenType.DIV);
                Term();
            }
            else if (_lookAhead?.Type == null)
                Match(ETokenType.EOL);
            else
                Error("Expected " + type + " -found" + _lookAhead?.Type);
        }


        private bool TestFollow(params ETokenType[] list)
        {
            return list.ToList().Exists(t => _lookAhead?.Type == t);
        }




    }
}