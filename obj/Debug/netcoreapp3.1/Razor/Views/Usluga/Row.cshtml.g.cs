#pragma checksum "D:\programskoprojekt\Views\Usluga\Row.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "fbc0c4f35bfd198baf40f49e54918962f287ff23"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Usluga_Row), @"mvc.1.0.view", @"/Views/Usluga/Row.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\programskoprojekt\Views\_ViewImports.cshtml"
using TestWebApp;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\programskoprojekt\Views\_ViewImports.cshtml"
using TestWebApp.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\programskoprojekt\Views\_ViewImports.cshtml"
using TestWebApp.ViewModels;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fbc0c4f35bfd198baf40f49e54918962f287ff23", @"/Views/Usluga/Row.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c3ef275df89311fe70bb54db4b4cfe35167ec386", @"/Views/_ViewImports.cshtml")]
    public class Views_Usluga_Row : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<TestWebApp.Models.Usluga>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Delete", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n<tr>\r\n\t\r\n\r\n\t<td class=\"text-left\">");
#nullable restore
#line 6 "D:\programskoprojekt\Views\Usluga\Row.cshtml"
                     Write(Model.Detalji);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n\t<td class=\"text-left\">");
#nullable restore
#line 7 "D:\programskoprojekt\Views\Usluga\Row.cshtml"
                     Write(Model.Cijena);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n\t<td class=\"text-left\">");
#nullable restore
#line 8 "D:\programskoprojekt\Views\Usluga\Row.cshtml"
                     Write(Model.Vrijeme);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n\t<td class=\"text-left\">");
#nullable restore
#line 9 "D:\programskoprojekt\Views\Usluga\Row.cshtml"
                     Write(Model.Kontakt);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n\t<td class=\"text-left\">");
#nullable restore
#line 10 "D:\programskoprojekt\Views\Usluga\Row.cshtml"
                     Write(Model.Lokacija);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n\t\r\n\t\r\n\t<td>\r\n\t\t<a data-id=\"");
#nullable restore
#line 14 "D:\programskoprojekt\Views\Usluga\Row.cshtml"
               Write(Model.Id);

#line default
#line hidden
#nullable disable
            WriteLiteral("\" class=\"btn btn-sm editajax\" title=\"Ažuriraj\">\r\n\t\t\t<i class=\"fas fa-edit\"></i>\r\n\t\t</a>\r\n\t</td>\r\n\t<td>\r\n\t\t");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "fbc0c4f35bfd198baf40f49e54918962f287ff235437", async() => {
                WriteLiteral("\r\n\t\t\t<input type=\"hidden\" name=\"Id\"");
                BeginWriteAttribute("value", " value=\"", 483, "\"", 500, 1);
#nullable restore
#line 20 "D:\programskoprojekt\Views\Usluga\Row.cshtml"
WriteAttributeValue("", 491, Model.Id, 491, 9, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n\t\t\t<button data-id=\"");
#nullable restore
#line 21 "D:\programskoprojekt\Views\Usluga\Row.cshtml"
                        Write(Model.Id);

#line default
#line hidden
#nullable disable
                WriteLiteral("\" type=\"submit\" class=\"btn btn-sm btn-danger deleteajax\" title=\"Obriši\">\r\n\t\t\t\t<i class=\"fas fa-trash-alt\"></i>\r\n\t\t\t</button>\r\n\t\t");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\t</td>\r\n</tr>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<TestWebApp.Models.Usluga> Html { get; private set; }
    }
}
#pragma warning restore 1591