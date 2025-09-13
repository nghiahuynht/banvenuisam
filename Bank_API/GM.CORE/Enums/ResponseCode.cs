namespace GM.CORE.Enums
{
    public enum ResponseCode
    {
        InternalServer = 1,
        UsernameExists,
        EmailExists,
        DepartmentExists = 501,
        CreateSccess = 201,
        EditSuccess = 202,

        ErrorOrther = 600
    }
}