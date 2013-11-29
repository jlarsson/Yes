namespace Yes.Parsing
{
    public class TryCatchFinallyParameters<TAst>
    {
        public TAst TryStatement { get; set; }
        public CatchParameters<TAst> CatchParameters { get; set; }
        public TAst FinallyStatement { get; set; }
    }
}