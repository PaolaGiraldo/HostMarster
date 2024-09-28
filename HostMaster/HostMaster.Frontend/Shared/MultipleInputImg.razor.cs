using HostMaster.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using System;

namespace HostMaster.Frontend.Shared;

public partial class MultipleInputImg
{
    private List<string> imagesBase64 = new List<string>(); // Lista para almacenar varias im�genes en Base64
    private List<string> fileNames = new List<string>();    // Lista para almacenar los nombres de archivos

    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    [Parameter] public string? Label { get; set; }
    [Parameter] public string? ImageURL { get; set; }       // Esto sigue siendo un solo URL
    [Parameter] public EventCallback<List<string>> ImagesSelected { get; set; }  // Cambiado a una lista de im�genes Base64

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        if (string.IsNullOrWhiteSpace(Label))
        {
            Label = Localizer["Image"];
        }
    }

    // Modificaci�n del m�todo OnChange para m�ltiples archivos
    // Modificaci�n del m�todo OnChange para aceptar un m�ximo de 3 archivos
    private async Task OnChange(InputFileChangeEventArgs e)
    {
        fileNames.Clear();      // Limpiar la lista de nombres de archivos
        imagesBase64.Clear();   // Limpiar la lista de im�genes previas

        var maxFiles = 3; // N�mero m�ximo de archivos permitidos
        var maxAllowedSize = 5 * 1024 * 1024; // Tama�o m�ximo permitido 5 MB

        var selectedFiles = e.GetMultipleFiles().Take(maxFiles); // Solo tomamos los primeros 3 archivos

        if (e.GetMultipleFiles().Count > maxFiles)
        {
            Console.WriteLine($"Solo se permiten un m�ximo de {maxFiles} archivos.");
        }

        foreach (var file in selectedFiles)  // Iterar sobre los archivos seleccionados (m�ximo 3)
        {
            try
            {
                fileNames.Add(file.Name);               // Agregar el nombre del archivo

                // Verificar que el archivo no exceda el tama�o permitido
                if (file.Size > maxAllowedSize)
                {
                    Console.WriteLine($"El archivo {file.Name} excede el tama�o m�ximo permitido de 5 MB.");
                    continue; // Saltar este archivo si excede el tama�o permitido
                }

                using var stream = file.OpenReadStream(maxAllowedSize);
                using var memoryStream = new MemoryStream();

                // Copiar el contenido del archivo en el MemoryStream
                await stream.CopyToAsync(memoryStream);
                byte[] arrBytes = memoryStream.ToArray();
                string base64String = Convert.ToBase64String(arrBytes); // Convertir a Base64
                imagesBase64.Add(base64String);         // Agregar a la lista de im�genes Base64
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error procesando el archivo {file.Name}: {ex.Message}");
            }
        }

        ImageURL = null;
        await ImagesSelected.InvokeAsync(imagesBase64);  // Invocar el callback con la lista de im�genes
        StateHasChanged();  // Notificar que el estado del componente ha cambiado
    }

}