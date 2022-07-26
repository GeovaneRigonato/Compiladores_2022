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
                Error();
        }

        public void Error()
        {
            Console.WriteLine("Lascou...");
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
                Error();
        }
        public void Input() //input     : INPUT VAR
        {
            if (_lookAhead?.Type == ETokenType.INPUT)
                Match(ETokenType.INPUT);
            else if (_lookAhead?.Type == ETokenType.VAR)
                Match(ETokenType.VAR);
            else
                Error();
        }
        
        public void Output() //output    : OUTPUT VAR
        {
            if (_lookAhead?.Type == ETokenType.OUTPUT)
                Match(ETokenType.OUTPUT);
            else if (_lookAhead?.Type == ETokenType.VAR)
                Match(ETokenType.VAR);
            else
                Error();
        }
        public void Atrib()//atrib  : VAR AT expr
        {
            if (_lookAhead?.Type == ETokenType.VAR)
                Match(ETokenType.VAR);
            else if (_lookAhead?.Type == ETokenType.AT)
                Match(ETokenType.AT);
            else if (_lookAhead?.Type == ETokenType.VAR)
                Expr();
            else
                Error();
        }
        public void Expr()//expr   : termY
        {
            term();
            Y();
        }

        public void term()//term   : factZ
        {
            Factz();
        }
        public void Y()//Y      : vazio | + expr | - expr
        {
            if (_lookAhead?.Type == ETokenType.SUM)
                Match(ETokenType.SUM);
            else if (_lookAhead?.Type == ETokenType.SUB)
                Match(ETokenType.SUB);
            else if (_lookAhead?.Type == null)
                Match(ETokenType.EOL);
            else
                Error();

        }
        public void Factz()//fact   : NUM | VAR | OE expr CE
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
                    Error();
            }
            else
                Error();
                

        }




    }
}