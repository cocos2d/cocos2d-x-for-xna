using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TemplateWizard;
using EnvDTE;
using Cocos2d_xna.Wizard;

namespace Cocos2d.Wizard
{
    public class Cocos2dWizard : IWizard
    {
        public void BeforeOpeningFile(ProjectItem projectItem)
        {

        }

        public void ProjectFinishedGenerating(Project project)
        {

        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {

        }

        public void RunFinished()
        {

        }

        public static bool CreateWithOpenxlive;
        public static string SolutionName;

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            try
            {
                OpenxliveComfirmForm form = new OpenxliveComfirmForm();
                form.ShowDialog();

                if (form.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    CreateWithOpenxlive = form.CreateWithOpenxlive;
                    SolutionName = replacementsDictionary["$projectname$"];

                    if (CreateWithOpenxlive)
                        replacementsDictionary.Add("$CreateWithOpenxlive$", "True");
                    else
                        replacementsDictionary.Add("$CreateWithOpenxlive$", "False");
                }
                else
                {
                    throw new WizardCancelledException();
                }
            }
            catch
            {

            }
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}
