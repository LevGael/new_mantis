using newMantis.Models;
using Newtonsoft.Json;
using newMantis.Configuration;

namespace newMantis
{
    public class CallMantisAPI
    {
        private readonly IMantisConfigManager _configuration;

        private AccessServer access;

        public CallMantisAPI(IMantisConfigManager configuration)
        {
            this._configuration = configuration;
            access = new AccessServer(configuration);
        }

        public async Task<Projet>getAllProjects()
        {
            Projet project = new Projet();

            try
            {
                string result = await access.callMethodAsync("projects");
                if(result == null)
                {
                    return null;
                }
                else
                {
                    project = JsonConvert.DeserializeObject<Projet>(result);
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException Caught!");	
                Console.WriteLine("Message :{0} ",e.Message);
            }
            return project;
        }

        public async Task<Projet> getAProject(int id)
        {
            Projet project = new Projet();

            try
            {
                string response = await access.callMethodAsync("projects/"+id+"&page_size=max");
                if(response == null)
                {
                    return null;
                }
                else
                {
                    project = JsonConvert.DeserializeObject<Projet>(response);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException Caught!");	
                Console.WriteLine("Message :{0} ",e.Message);
            }

            return project;
        }

        public async Task<Issue> getIssuesFromProject(int id)
        {
            Issue issues = new Issue();

            try
            {
                string response = await access.callMethodAsync("issues?project_id="+id+"&page_size=max");

                if(response == null)
                {
                    return null;
                }
                else
                {
                    issues = JsonConvert.DeserializeObject<Issue>(response);
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException Caught!");	
                Console.WriteLine("Message :{0} ",e.Message);
            }

            return issues;
        }

    }
        
}