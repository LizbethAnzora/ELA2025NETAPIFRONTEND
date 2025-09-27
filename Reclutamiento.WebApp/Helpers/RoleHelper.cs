namespace FrontAuth.WebApp.Helpers
{
    public static class RoleHelper
    {
        public static string GetRoleName(object rol)
        {
            if (rol == null) return "Solicitante";
            if (rol is string strRol)
            {
                if (strRol == "Admin" || strRol == "1")
                    return "Admin";
            }
            if (rol is int intRol)
            {
                if (intRol == 1)
                    return "Admin";
            }
            // Si el claim viene como JsonElement string
            if (rol is System.Text.Json.JsonElement json)
            {
                if (json.ValueKind == System.Text.Json.JsonValueKind.String && json.GetString() == "Admin")
                    return "Admin";
                if (json.ValueKind == System.Text.Json.JsonValueKind.String && json.GetString() == "1")
                    return "Admin";
                if (json.ValueKind == System.Text.Json.JsonValueKind.Number && json.GetInt32() == 1)
                    return "Admin";
            }
            return "Solicitante";
        }

        // Método recursivo eliminado: ya no es necesario
    }
}