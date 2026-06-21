using System.Xml.Linq;

namespace SpravaProjektovAPI.Model
{
    public class XMLRepository : IProjectRepository
    {
        private readonly string _filePath;

        public XMLRepository(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("filePath must be provided", nameof(filePath));

            _filePath = filePath;
        }

        public async Task<Project> AddAsync(Project project)
        {
            ArgumentNullException.ThrowIfNull(project);

            if (string.IsNullOrWhiteSpace(project.Id)) throw new ArgumentException("project.Id must be provided", nameof(project.Id));

            var doc = await LoadDocumentAsync() ?? throw new NullReferenceException("XML document is null.");

            // ensure root exists (create if missing)
            if (doc.Root == null)
            {
                doc.Add(new XElement("projects"));
            }

            var root = doc.Root!; // now guaranteed non-null

            var element = new XElement("project",
                new XAttribute("id", project.Id),
                new XElement("name", project.Name ?? string.Empty),
                new XElement("abbreviation", project.Abbreviation ?? string.Empty),
                new XElement("customer", project.Customer ?? string.Empty)
            );

            root.Add(element);

            await SaveDocumentAsync(doc);

            return project;
        }

        public async Task DeleteAsync(string key)      
        {
            var doc = await LoadDocumentAsync();

            if (doc == null || doc.Root == null) throw new NullReferenceException("XML document is null.");

            if(string.IsNullOrEmpty(key)) throw new ArgumentException("key must be provided", nameof(key));
            

            var node = doc.Root.Elements("project")
                .FirstOrDefault(x => (string?)x.Attribute("id") == key);

            node?.Remove();

            await SaveDocumentAsync(doc);
        }

        public async Task<List<Project>> GetAllAsync()
        {
            var doc = await LoadDocumentAsync();

            if (doc == null || doc.Root == null) throw new NullReferenceException("XML document is null.");

            return doc.Root
                .Elements("project")
                .Select(x => new Project
                {
                    Id = (string?)x.Attribute("id"),
                    Name = (string?)x.Element("name"),
                    Abbreviation = (string?)x.Element("abbreviation"),
                    Customer = (string?)x.Element("customer")
                })
                .ToList();
        }

        public async Task<Project?> GetByIdAsync(string key)
        {
            var doc = await LoadDocumentAsync();

            if (doc == null || doc.Root == null) throw new NullReferenceException("XML document is null.");

            if (string.IsNullOrEmpty(key)) throw new ArgumentException("key must be provided", nameof(key));

            return doc.Root.Elements("project")
                .Select(x => new Project
                {
                    Id = (string?)x.Attribute("id"),
                    Name = (string?)x.Element("name"),
                    Abbreviation = (string?)x.Element("abbreviation"),
                    Customer = (string?)x.Element("customer")
                })
                .FirstOrDefault(x => x.Id == key);
        }

        public async Task UpdateAsync(Project project)
        {
            ArgumentNullException.ThrowIfNull(project);
            if (string.IsNullOrWhiteSpace(project.Id)) throw new ArgumentException("project.Id must be provided", nameof(project.Id));

            var doc = await LoadDocumentAsync();

            if (doc == null || doc.Root == null) throw new NullReferenceException("XML document is null.");
                     

            var node = doc.Root.Elements("project")
                .FirstOrDefault(x => (string?)x.Attribute("id") == project.Id);

            if (node == null) return;

            var nameEl = node.Element("name");
            if (nameEl == null) node.Add(new XElement("name", project.Name ?? string.Empty));
            else nameEl.Value = project.Name ?? string.Empty;

            var abbrEl = node.Element("abbreviation");
            if (abbrEl == null) node.Add(new XElement("abbreviation", project.Abbreviation ?? string.Empty));
            else abbrEl.Value = project.Abbreviation ?? string.Empty;

            var custEl = node.Element("customer");
            if (custEl == null) node.Add(new XElement("customer", project.Customer ?? string.Empty));
            else custEl.Value = project.Customer ?? string.Empty;

            await SaveDocumentAsync(doc);
        }

        private async Task<XDocument> LoadDocumentAsync()
        {
            await using var stream = File.OpenRead(_filePath);

            return await XDocument.LoadAsync(
                stream,
                LoadOptions.None,
                CancellationToken.None);
        }

        private async Task SaveDocumentAsync(XDocument document)
        {
            await using var stream = File.Create(_filePath);

            await document.SaveAsync(
                stream,
                SaveOptions.None,
                CancellationToken.None);
        }
    } 
}
