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
        return Task.FromResult(GenerateSyntheticToolCall(query, @"
**Key sections to include:**
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
        return Task.FromResult(GenerateSyntheticToolCall(query, @"
**Key sections to include:**
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
        return Task.FromResult(GenerateSyntheticToolCall(query, @"
**Key sections to include:**
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
        return Task.FromResult(GenerateSyntheticToolCall(query, @"
**Key sections to include:**
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
        return Task.FromResult(GenerateSyntheticToolCall(query, @"
**Key sections to include:**
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
        return Task.FromResult(GenerateSyntheticToolCall(query, @"
**Key sections to include:**
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
        return Task.FromResult(GenerateSyntheticToolCall(query, @"
**Key sections to include:**
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
        return Task.FromResult(GenerateSyntheticToolCall(query, @"
**Key sections to include:**
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
        return Task.FromResult(GenerateSyntheticToolCall(query, @"
**Key sections to include:**
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
        return Task.FromResult(GenerateSyntheticToolCall(query, @"
**Key sections to include:**
- 🎯 Integration overview and business purpose
- 🏗️ Architecture patterns and alternatives
- 🛠️ Implementation guide with code examples
- 🔍 Testing and monitoring strategies
- 📋 Best practices and error handling"));
    }

    private static string GenerateSyntheticToolCall(string query, string format)
    {
        var parts = new List<string>
        {
            // Add the main search query
            $"📋 **Search Query:** `{query}`"
        };
        
        // Create the tool call instruction with format guidance
        var toolCallInstruction = $@"## 🔍 Search Knowledge Base

{string.Join("\n", parts)}

**To execute this search, use:**
```
#Retrieve ""{query}""
```

*This will search your organization's knowledge repositories for relevant documentation and resources.*

---

## 📝 Expected Response Format

After executing the tool call, please format the response using the following template:

{format}";

        return toolCallInstruction;
    }
}