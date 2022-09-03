/* Méthode tri tableau */

/* 

Cette fonction permet de trier la liste de projet récupérer sur l'api Mantis. Comme chaque sous projet d'un projet était considéré comme
un projet à part entière, nous avions des doublons lors de l'affichage de liste dans l'Onglet Général de l'application. Cette fonction 
permet donc de rétablir les relations père-fils entre les projets en utilisant le model "Projet".

Nous avons développé cette première solution dans le but d'avoir un tableau trié qui serait plus facile à afficher. 

Finalement, il existe une autre solution qui utilise les jstree et qui est encore plus simple à mettre en place. Nous sommes donc parti
sur cette solution car finalement nous n'avions pas besoin du model "Projet" pour pouvoir afficher les projets dans l'onglet Général. 

*/

function Projet tri-projet(Projet allprojet)
{
    CallMantisAPI apiMantis = new CallMantisAPI(_configuration);
    Projet Projets = new Projet();

    if(allProject.projects.Count > 0)
    {
    
        for (int i = 0; i <allProject.projects.Count; i++)
        {
            if(allProject.projects[i].subprojects == null)
            {
                Projets.projects.Add(allProject.projects[i]);
                allProject.projects.Remove(allProject.projects[i]);
                i--;
            }
        }

        while(allProject.projects.Count>0)
        {
            if(cpt >= allProject.projects.Count)
            {
                cpt = 0;
            }

            List<projects> subs = new List<projects>();
            projects sub = new projects();
        
            foreach(projects subprojet in allProject.projects[cpt].subprojects)
            {
                if (Projets.projects.Find(s => s.id == subprojet.id) != null )
                {
                    sub = Projets.projects.Find(s => s.id == subprojet.id)!;
                    subs.Add(sub);
                }
            }
            if(subs.Count == allProject.projects[cpt].subprojects.Count)
            {
                allProject.projects[cpt].subprojects = subs;
                foreach(projects s in subs)
                {
                    Projets.projects.Remove(s);
                }
                Projets.projects.Add(allProject.projects[cpt]);
                allProject.projects.Remove(allProject.projects[cpt]);
            }
            else
            {
                cpt ++;
            }
        }
    }

    return Projets;

}