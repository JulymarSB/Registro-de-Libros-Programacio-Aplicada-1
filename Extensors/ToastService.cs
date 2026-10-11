using Blazored.Toast.Services;

namespace RegistroLibros.Extensors;

public static class ToastExtension
{
    public static void MostrarExito(this IToastService toast, string mensaje)
    {
        toast.ShowSuccess(mensaje);
    }

    public static void MostrarError(this IToastService toast, string mensaje)
    {
        toast.ShowError(mensaje);
    }
}