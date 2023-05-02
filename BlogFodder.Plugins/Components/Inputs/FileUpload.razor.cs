using System.Linq.Expressions;
using System.Security.Claims;
using BlogFodder.Core.Providers;
using BlogFodder.Core.Settings;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;

namespace BlogFodder.Plugins.Components.Inputs;

public partial class FileUpload : ComponentBase
    {
        [Parameter] public int MaxAllowedFiles { get; set; } = 1; // Single File Initially
        [Parameter] public bool ImagesOnly { get; set; } = false;
        [Parameter] public string InputId { get; set; } = "input-file";
        [Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; }
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [CascadingParameter] private EditContext CascadedEditContext { get; set; }

        [Parameter] public IBrowserFile Value { get; set; }
        [Parameter] public EventCallback<IBrowserFile> ValueChanged { get; set; }
        [Parameter] public Expression<Func<IBrowserFile>> ValueExpression { get; set; }

        [Parameter] public List<IBrowserFile> Values { get; set; }
        [Parameter] public EventCallback<List<IBrowserFile>> ValuesChanged { get; set; }
        [Parameter] public Expression<Func<List<IBrowserFile>>> ValuesExpression { get; set; }

        [Inject] public IUiNotificationService NotificationService { get; set; }
        [Inject] private ProviderService ProviderService { get; set; }
        [Inject] public IOptionsSnapshot<BlogFodderSettings> Settings { get; set; }

        private EditContext _editContext;
        private FieldIdentifier _fieldIdentifier;

        private ClaimsPrincipal? User { get; set; }
        private bool ShowSpinner { get; set; }

        private bool IsMultiselect => ValuesExpression != null;

        protected override async Task OnInitializedAsync()
        {
            _editContext = CascadedEditContext;
            _fieldIdentifier = IsMultiselect ? FieldIdentifier.Create(ValuesExpression) : FieldIdentifier.Create(ValueExpression);
            var authState = await AuthenticationStateTask.ConfigureAwait(false);
            User = authState.User;
        }

        private async Task OnChange(InputFileChangeEventArgs e)
        {
            ShowSpinner = true;
            if(e.FileCount > MaxAllowedFiles)
            {
                NotificationService.ShowError($"You can only upload {MaxAllowedFiles} files");
            }
            else
            {
                if (MaxAllowedFiles > 1)
                {
                    var files = e.GetMultipleFiles(MaxAllowedFiles);

                    try
                    {
                        var allowedToUseFiles = new List<IBrowserFile>();
                        foreach (var file in files)
                        {
                            var result = ProviderService.StorageProvider.CanUseFile(file, ImagesOnly).Result;
                            if (result.Success)
                            {
                                allowedToUseFiles.Add(file);
                            }
                            else
                            {
                                allowedToUseFiles.Clear();
                                await ClearFiles().ConfigureAwait(false);
                                NotificationService.ShowError(result.ErrorMessages);
                                break;
                            }
                        }
                        if (allowedToUseFiles.Any())
                        {
                            await ValuesChanged.InvokeAsync(allowedToUseFiles).ConfigureAwait(false);
                            _editContext?.NotifyFieldChanged(_fieldIdentifier);
                            StateHasChanged();
                        }
                    }
                    catch (Exception ex)
                    {
                        await ClearFiles().ConfigureAwait(false);
                        NotificationService.ShowError(ex.Message);
                    }
                }
                else
                {
                    var result = ProviderService.StorageProvider.CanUseFile(e.File, ImagesOnly).Result;
                    if (result.Success)
                    {
                        await ValueChanged.InvokeAsync(e.File).ConfigureAwait(false);
                        _editContext?.NotifyFieldChanged(_fieldIdentifier);
                        StateHasChanged();
                    }
                    else
                    {
                        await ClearFiles().ConfigureAwait(false);
                        NotificationService.ShowError(result.ErrorMessages);
                    }
                }
            }
            ShowSpinner = false;
        }

        private async Task ClearFiles()
        {
            if (IsMultiselect)
            {
                await ValuesChanged.InvokeAsync(new List<IBrowserFile>()).ConfigureAwait(false);
            }
            else
            {
                await ValueChanged.InvokeAsync(default).ConfigureAwait(false);
            }

            _editContext?.NotifyFieldChanged(_fieldIdentifier);
        }

        private async Task RemoveValue(IBrowserFile item)
        {
            var valueList = Values ?? new List<IBrowserFile>();
            if (valueList.Contains(item))
            {
                valueList.Remove(item);
                await ValuesChanged.InvokeAsync(valueList).ConfigureAwait(false);
            }
            else if(item.Equals(Value))
            {
                await ValueChanged.InvokeAsync(default).ConfigureAwait(false);
            }

            _editContext?.NotifyFieldChanged(_fieldIdentifier);
        }


        //async Task LoadImage(InputFileChangeEventArgs e)
        //{
        //    var format = "image/jpeg";
        //    var imageFile = await e.File.RequestImageFileAsync(format, 640, 480);

        //    using var fileStream = imageFile.OpenReadStream(maxFileSize);
        //    using var memoryStream = new MemoryStream();
        //    await fileStream.CopyToAsync(memoryStream);

        //    imageDataUri = $"data:{format};base64,{Convert.ToBase64String(memoryStream.ToArray())}";
        //}
    }