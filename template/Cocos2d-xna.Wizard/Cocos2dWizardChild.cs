using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TemplateWizard;

namespace Cocos2d.Wizard
{
    public class Cocos2dWizardChild : IWizard
    {
        public void BeforeOpeningFile(EnvDTE.ProjectItem projectItem)
        {

        }

        public void ProjectFinishedGenerating(EnvDTE.Project project)
        {

        }

        public void ProjectItemFinishedGenerating(EnvDTE.ProjectItem projectItem)
        {

        }

        public void RunFinished()
        {

        }

        bool CreateWithOpenxlive;
        string SolutionName;
        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            CreateWithOpenxlive = Cocos2dWizard.CreateWithOpenxlive;
            SolutionName = Cocos2dWizard.SolutionName;

            if (CreateWithOpenxlive)
                replacementsDictionary.Add("$CreateWithOpenxlive$", "True");
            else
                replacementsDictionary.Add("$CreateWithOpenxlive$", "False");

            replacementsDictionary.Add("$SolutionName$", SolutionName.Replace('-', '_'));
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            if (!CreateWithOpenxlive)
            {
                if (filePath.EndsWith("OpenXLive.dll".ToLower()))
                {
                    return false;
                }

                if (filePath.EndsWith("OpenXLive.Form.dll".ToLower()))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
