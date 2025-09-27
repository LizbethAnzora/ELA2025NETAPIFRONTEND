namespace FrontAuth.WebApp.Helpers
{
    public static class RoleHelper
    {
        public static string GetRoleName(object rol)
        {
            if (rol == null) return "Solicitante";
            // Si es int y es 1
            if (rol is int intRol)
                return intRol == 1 ? "Admin" : "Solicitante";
            // Si es string
            if (rol is string strRol)
            {
                var normalized = strRol.Trim().ToLower();
                if (normalized == "admin" || normalized == "1" || normalized.Contains("admin"))
                    return "Admin";
            }
            // Si es un objeto complejo con propiedad Nombre/nombre o Id/id
            var type = rol.GetType();
            var nombreProp = type.GetProperty("Nombre") ?? type.GetProperty("nombre");
            if (nombreProp != null)
            {
                var nombreValue = nombreProp.GetValue(rol)?.ToString()?.Trim().ToLower();
                if (!string.IsNullOrEmpty(nombreValue) && (nombreValue == "admin" || nombreValue.Contains("admin")))
                    return "Admin";
            }
            var idProp = type.GetProperty("Id") ?? type.GetProperty("id");
            if (idProp != null)
            {
                var idValue = idProp.GetValue(rol)?.ToString()?.Trim();
                if (idValue == "1")
                    return "Admin";
            }
            // Si el ToString() del objeto contiene admin
            if (rol.ToString()?.ToLower().Contains("admin") == true)
                return "Admin";
            return "Solicitante";
        }
    }
}