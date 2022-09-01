using VerySimpleInterpreter.Lexer;
using VerySimpleInterpreter;
using VerySimpleInterpreter.Parser;

var st = new SymbolTable();
var basicLexer = new BasicLexer("docs/example.lang", st);
var basicParser = new BasicParser(basicLexer, st);

basicParser.Prog();