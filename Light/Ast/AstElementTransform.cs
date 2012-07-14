namespace Light.Ast {
    public delegate IAstElement AstElementTransform(IAstElement element);
    public delegate IAstElement AstElementTransform<in TAstElement>(TAstElement element);
}