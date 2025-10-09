using System.ComponentModel;
using ModelContextProtocol.Server;

namespace M365.CoPilot.MCP;

[McpServerPromptType]
public class CoPilotPrompts()
{
    [McpServerPrompt(Name = "project_research")]
    [Description("Research project documentation, best practices, and technical resources across your organization's knowledge base. Ideal for finding development standards, architecture decisions, and project-related documentation.")]
    public Task<string> ProjectResearchAsync(
        [Description("The project, technology, or topic you want to research (e.g., 'mobile app development', 'API design patterns', 'deployment strategies')")]
        string topic)
    {
        var query = $"project documentation best practices guidelines standards for {topic}";
        return Task.FromResult(GenerateResearchPrompt(query, topic, @"
- 🎯 Overview and relevance
- 🏗️ Architecture & design principles  
- 📋 Development standards and best practices
- 🔗 Key resources and documentation links
- ✅ Actionable next steps"));
    }

    [McpServerPrompt(Name = "troubleshooting_guide")]
    [Description("Find troubleshooting guides, error solutions, and technical documentation for resolving common issues. Perfect for locating debugging procedures and problem resolution steps.")]
    public Task<string> TroubleshootingGuideAsync(
        [Description("The error, issue, or problem you need help troubleshooting (e.g., 'authentication errors', 'deployment failures', 'database connection issues')")]
        string issue)
    {
        var query = $"troubleshooting guide solution fix error resolution for {issue}";
        return Task.FromResult(GenerateResearchPrompt(query, issue, @"
- 🔍 Problem description and symptoms
- 🎯 Quick diagnosis steps
- 🛠️ Solution steps (primary and alternative)
- 🔧 Prevention measures
- 📞 Escalation path"));
    }

    [McpServerPrompt(Name = "code_standards")]
    [Description("Access coding standards, architecture guidelines, and technical best practices from your organization. Useful for understanding development conventions and architectural decisions.")]
    public Task<string> CodeStandardsAsync(
        [Description("The technology, framework, or area you need standards for (e.g., 'React development', 'microservices architecture', 'API design', 'security guidelines')")]
        string technology)
    {
        var query = $"coding standards guidelines architecture best practices conventions for {technology}";
        return Task.FromResult(GenerateResearchPrompt(query, technology, @"
- 🏗️ Architecture guidelines and patterns
- 📝 Coding conventions and style
- 🔍 Code quality standards and review checklist
- 🛠️ Tools and configuration
- 📚 Examples and templates"));
    }

    [McpServerPrompt(Name = "business_process")]
    [Description("Retrieve business process documentation, workflows, and operational procedures from your organization. Perfect for understanding company processes and compliance requirements.")]
    public Task<string> BusinessProcessAsync(
        [Description("The business process or workflow you need information about (e.g., 'customer onboarding', 'expense approval', 'vendor management', 'data privacy')")]
        string process)
    {
        var query = $"business process workflow procedure documentation for {process}";
        return Task.FromResult(GenerateResearchPrompt(query, process, @"
- 📋 Process overview and stakeholders
- 🔄 Process flow with clear phases
- 👥 Roles and responsibilities
- 📊 Success metrics and KPIs
- ⚠️ Compliance and risk considerations"));
    }

    [McpServerPrompt(Name = "competitive_intelligence")]
    [Description("Research market analysis, competitor information, and business intelligence from internal reports and documents. Ideal for gathering market insights and competitive positioning.")]
    public Task<string> CompetitiveIntelligenceAsync(
        [Description("The market, competitor, or business area you want to research (e.g., 'mobile payment market', 'competitor analysis Q3', 'customer satisfaction trends')")]
        string topic)
    {
        var query = $"market analysis competitor intelligence business research insights for {topic}";
        return Task.FromResult(GenerateResearchPrompt(query, topic, @"
- 🎯 Executive summary with key insights
- 🏪 Market landscape and positioning
- 🏢 Competitor analysis and threats
- 💡 Strategic insights and opportunities
- 📈 Key metrics and benchmarks"));
    }

    [McpServerPrompt(Name = "devops_procedures")]
    [Description("Access DevOps procedures, deployment guides, and infrastructure documentation. Perfect for finding CI/CD pipelines, deployment strategies, and operational procedures.")]
    public Task<string> DevOpsProceduresAsync(
        [Description("The DevOps area or procedure you need information about (e.g., 'CI/CD pipeline', 'container deployment', 'monitoring setup', 'infrastructure automation')")]
        string procedure)
    {
        var query = $"DevOps procedures deployment guide infrastructure documentation for {procedure}";
        return Task.FromResult(GenerateResearchPrompt(query, procedure, @"
- 🎯 Procedure overview and prerequisites
- 🚀 Step-by-step process (prep, execution, post)
- 🛠️ Tools and resources required
- ⚠️ Troubleshooting and common issues
- 📞 Support and escalation contacts"));
    }

    [McpServerPrompt(Name = "compliance_documentation")]
    [Description("Find compliance procedures, audit documentation, and regulatory guidelines from your organization. Essential for understanding security, privacy, and regulatory requirements.")]
    public Task<string> ComplianceDocumentationAsync(
        [Description("The compliance area or regulation you need information about (e.g., 'GDPR compliance', 'security audit', 'data retention', 'privacy policies')")]
        string complianceArea)
    {
        var query = $"compliance documentation audit procedures regulatory guidelines for {complianceArea}";
        return Task.FromResult(GenerateResearchPrompt(query, complianceArea, @"
- 📋 Compliance overview and scope
- 📜 Regulatory requirements and standards
- ✅ Compliance checklist and procedures
- 📊 Audit and monitoring processes
- 🚨 Non-compliance response plan"));
    }

    [McpServerPrompt(Name = "training_resources")]
    [Description("Access training materials, onboarding guides, and learning resources from your organization. Perfect for finding educational content and skill development materials.")]
    public Task<string> TrainingResourcesAsync(
        [Description("The training topic or skill area you need resources for (e.g., 'new employee onboarding', 'cloud platform training', 'security awareness', 'technical certifications')")]
        string trainingTopic)
    {
        var query = $"training resources learning materials onboarding guides for {trainingTopic}";
        return Task.FromResult(GenerateResearchPrompt(query, trainingTopic, @"
- 🎯 Learning objectives and outcomes
- 📚 Core learning materials and resources
- 🛤️ Structured learning path by level
- 🏆 Assessments and certification options
- 🤝 Support and community resources"));
    }

    [McpServerPrompt(Name = "customer_insights")]
    [Description("Research customer feedback, survey results, and user research findings from your organization's knowledge base. Ideal for understanding customer needs and satisfaction trends.")]
    public Task<string> CustomerInsightsAsync(
        [Description("The customer insight area you want to research (e.g., 'customer satisfaction', 'user experience feedback', 'product reviews', 'support ticket trends')")]
        string insightArea)
    {
        var query = $"customer insights feedback survey results user research findings for {insightArea}";
        return Task.FromResult(GenerateResearchPrompt(query, insightArea, @"
- 📊 Executive summary with key findings
- 🔍 Customer feedback analysis and metrics
- 📈 Trends and behavioral patterns
- 💡 Actionable insights and opportunities
- 📋 Recommendations with timelines"));
    }

    [McpServerPrompt(Name = "integration_patterns")]
    [Description("Find integration patterns, API documentation, and system integration guides from your organization's technical resources. Perfect for understanding system connections and data flows.")]
    public Task<string> IntegrationPatternsAsync(
        [Description("The integration area or system you need patterns for (e.g., 'payment system integration', 'webhook implementation', 'data synchronization', 'third-party APIs')")]
        string integrationArea)
    {
        var query = $"integration patterns API documentation system integration guides for {integrationArea}";
        return Task.FromResult(GenerateResearchPrompt(query, integrationArea, @"
- 🎯 Integration overview and business purpose
- 🏗️ Architecture patterns and alternatives
- 🛠️ Implementation guide with code examples
- 🔍 Testing and monitoring strategies
- 📋 Best practices and error handling"));
    }

    private static string GenerateResearchPrompt(string query, string topic, string format)
    {
        return $@"You are conducting research on ""{topic}"". Please perform the following research steps and provide a comprehensive response:

## Step 1: Search Internal Knowledge Base
Use the Retrieve tool to search for internal documentation and resources:

```
#Retrieve: ""{query}""
```

## Step 2: Gather Additional Web Context  
Use the get_web_pages tool to research current best practices and industry standards related to ""{topic}"". Search for:
- 🔍 Recent industry articles and documentation
- 🧱 Official framework/technology documentation
- 📘 Best practice guides from reputable sources

## Step 3: Synthesize Information
Combine the internal and external research to provide a comprehensive response in this format:{format}
- 📎 Provide Sources and References
   - 🔗 **Internal Sources:** [Title](URL) - Last updated: Date
   - 🌐 **External Sources:** [Title](URL) - Accessed: Date  
   - 📋 **Related Documentation:** Links to additional relevant resources
   - ⚡ **Quick Access:** Direct links to most actionable content

## Research Quality Requirements:
✔️ All claims must be backed by credible sources
✔️ Provide working links for all referenced materials
✔️ Ensure information is current and relevant
✔️ Clearly identify any gaps in available knowledge
✔️ Include specific, actionable next steps

Begin your research now by executing the #Retrieve tool with the query: ""{query}""";
    }
}