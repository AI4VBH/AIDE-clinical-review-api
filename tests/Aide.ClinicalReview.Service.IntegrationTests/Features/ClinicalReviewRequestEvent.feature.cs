﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (https://www.specflow.org/).
//      SpecFlow Version:3.9.0.0
//      SpecFlow Generator Version:3.9.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Aide.ClinicalReview.Service.IntegrationTests.Features
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.9.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("ClinicalReviewRequestEvent")]
    public partial class ClinicalReviewRequestEventFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = ((string[])(null));
        
#line 1 "ClinicalReviewRequestEvent.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features", "ClinicalReviewRequestEvent", null, ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.OneTimeTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<NUnit.Framework.TestContext>(NUnit.Framework.TestContext.CurrentContext);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Publish a clinical review request event and see the event and study details are s" +
            "aved in MongoDB")]
        [NUnit.Framework.CategoryAttribute("ClinicalReviewRequestEvent")]
        [NUnit.Framework.TestCaseAttribute("study/dcm/series/, study/workflows/task1/execution1/", "ClinicalReviewRequestEvent_1.json", "ExpectedClinicalReviewStudy_1.json", null)]
        [NUnit.Framework.TestCaseAttribute("study/dcm/series/, study/workflows/task2/execution1/", "ClinicalReviewRequestEvent_2.json", "ExpectedClinicalReviewStudy_2.json", null)]
        [NUnit.Framework.TestCaseAttribute("study/dcm/series/, study/workflows/task1/execution1/, study/workflows/task2/execu" +
            "tion1/", "ClinicalReviewRequestEvent_1.json", "ExpectedClinicalReviewStudy_1.json", null)]
        [NUnit.Framework.TestCaseAttribute("study/dcm/series/, study/workflows/task3/execution1/", "ClinicalReviewRequestEvent_3.json", "ExpectedClinicalReviewStudy_3.json", null)]
        [NUnit.Framework.TestCaseAttribute("study/dcm/series/, study/workflows/task4/execution1/", "ClinicalReviewRequestEvent_4.json", "ExpectedClinicalReviewStudy_4.json", null)]
        public virtual void PublishAClinicalReviewRequestEventAndSeeTheEventAndStudyDetailsAreSavedInMongoDB(string paths, string clinicialReviewEvent, string studyDetails, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "ClinicalReviewRequestEvent"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            string[] tagsOfScenario = @__tags;
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            argumentsOfScenario.Add("paths", paths);
            argumentsOfScenario.Add("clinicialReviewEvent", clinicialReviewEvent);
            argumentsOfScenario.Add("studyDetails", studyDetails);
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Publish a clinical review request event and see the event and study details are s" +
                    "aved in MongoDB", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 18
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 19
 testRunner.Given(string.Format("I have artifacts in minio {0}", paths), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 20
 testRunner.When(string.Format("I publish a Clinical Review Request Event {0}", clinicialReviewEvent), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 21
 testRunner.Then("I can see ClinicalReviewRecord in Mongo", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 22
 testRunner.And(string.Format("I can see StudyRecord in Mongo matches {0}", studyDetails), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Publish a clinical review request event and see thecorrect roles are applied")]
        [NUnit.Framework.CategoryAttribute("ClinicalReviewRequestEvent")]
        [NUnit.Framework.TestCaseAttribute("study/dcm/series/, study/workflows/task1/execution1/", "ClinicalReviewRequestEvent_5.json", null)]
        [NUnit.Framework.TestCaseAttribute("study/dcm/series/, study/workflows/task1/execution1/", "ClinicalReviewRequestEvent_6.json", null)]
        [NUnit.Framework.TestCaseAttribute("study/dcm/series/, study/workflows/task1/execution1/", "ClinicalReviewRequestEvent_7.json", null)]
        public virtual void PublishAClinicalReviewRequestEventAndSeeThecorrectRolesAreApplied(string paths, string name, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "ClinicalReviewRequestEvent"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            string[] tagsOfScenario = @__tags;
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            argumentsOfScenario.Add("paths", paths);
            argumentsOfScenario.Add("name", name);
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Publish a clinical review request event and see thecorrect roles are applied", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 32
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 33
 testRunner.Given(string.Format("I have artifacts in minio {0}", paths), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 34
 testRunner.When(string.Format("I publish a Clinical Review Request Event {0}", name), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 35
 testRunner.Then("I can see the correct roles are applied", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
