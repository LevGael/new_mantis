using System.Data;
using Microsoft.AspNetCore.Mvc;
using newMantis.Models;
using newMantis.Configuration;
using Microsoft.Extensions.FileProviders;


namespace newMantis.Controllers;
/// Classe Excel
///
/// Description : Cette classe permet de générer un fichier Excel sur les issues
///
/// Créé : 27/05/2022 Levanier
///
/// Notes : La création de ce fichier dépend des paramètres choisi dans la vue General
public class ExcelController : Controller
{
    /**
    * Méthode CreateCSVFile
    *
    *      cette méthode permet de passer des paramètres avec un fichier en destination du serveur
    *
    *
    * @param type_client           nécessaire par rapport à URI du fichier pour pouvoir le uploader
    * @param est_detaille            Permet d'avoir une version plus détaillé du rapport ou non
    * @param list_projets                La liste des projets selectionnées dans la page général.
    * @param datedeb et datefin  Permettent de sélectionner les issues dont la date
    *
    * Créé : 05/2022 Levanier
    */

    private readonly IFileProvider fileProvider;

    private readonly IMantisConfigManager _configuration;

    CallMantisAPI callApi;

    static string Namefile = "" ;

    public ExcelController(IMantisConfigManager configuration)
    {
        _configuration = configuration;
        callApi = new CallMantisAPI(configuration);
    }

    [HttpPost]

    public void CreateCSVFile(string type_client, bool est_detaille, Projet[] list_projets, DateTime datedeb, DateTime datefi)
    {

        DateOnly d1 = DateOnly.FromDateTime(DateTime.Now);


        string currentDyr = Directory.GetCurrentDirectory();

        string name_file = "";

        for (int i = 0; i < list_projets.Length; i++)
        {
            name_file += list_projets[i].projects[0].name;
            if(i != list_projets.Length-1)
            {
                name_file += "-";
            }
        }


        string strFilePath = Path.Combine(currentDyr, "Historique/bilan_"+ name_file + ".csv");
        Namefile = "bilan_" + name_file + ".csv";

        // Create a new DataTable.
        System.Data.DataTable table = new DataTable("ParentTable");

        // Declare variables for DataColumn and DataRow objects. 
        DataColumn column;
        DataRow row;

        column = new DataColumn();
        column.DataType = System.Type.GetType("System.String");
        column.ColumnName = "Nom du projet";
        column.ReadOnly = true;
        column.Unique = false;
        // Add the Column to the DataColumnCollection.
        table.Columns.Add(column);
        // Create new DataColumn, set DataType, ColumnName and add to DataTable.
        column = new DataColumn();
        column.DataType = System.Type.GetType("System.String");
        column.ColumnName = "issue";
        column.ReadOnly = true;
        column.Unique = false;
        // Add the Column to the DataColumnCollection.
        table.Columns.Add(column);

        // Create second column.
        column = new DataColumn();
        column.DataType = System.Type.GetType("System.Int32");
        column.ColumnName = "total";
        column.AutoIncrement = false;
        column.Caption = "total";
        column.ReadOnly = false;
        column.Unique = false;
        // Add the column to the table.
        table.Columns.Add(column);

        // Make the ID column the primary key column.

        // Instantiate the DataSet variable.
        DataSet dataSet = new DataSet();
        // Add the new DataTable to the DataSet.
        dataSet.Tables.Add(table);

        foreach (Projet un_projet in list_projets)
        {
            int issueInterne = 0;
            int nonresoluInterne = 0;
            int issueExterne = 0;
            int nonresoluExterne = 0;
            int issue = 0;

            int nbdate = 0;
            int nbrecorect = 0;

            int mretard = 0;
            int Mretard = 0;
            int cretard = 0;
            int mretardfini = 0;
            int Mretardfini = 0;
            int cretardfini = 0;

            int smineur = 0;
            int smajeur = 0;
            int scritique = 0;

            int mEvolution = 0;
            int mBug = 0;
            int mPlantage = 0;
            int mTMA = 0;

            int MEvolution = 0;
            int MBug = 0;
            int MPlantage = 0;
            int MTMA = 0;

            int cEvolution = 0;
            int cBug = 0;
            int cPlantage = 0;
            int cTMA = 0;

            int mopen = 0;
            int mcorrige = 0;
            int mreopen = 0;
            int mimpossibleReproduire = 0;
            int mimpossibleCorriger = 0;
            int mdoublon = 0;
            int mnonAnormalie = 0;
            int msuspendu = 0;
            int mnonCorrige = 0;
            int mautreInterne = 0;
            int mautreExterne = 0;

            int Mopen = 0;
            int Mcorrige = 0;
            int Mreopen = 0;
            int MimpossibleReproduire = 0;
            int MimpossibleCorriger = 0;
            int Mdoublon = 0;
            int MnonAnormalie = 0;
            int Msuspendu = 0;
            int MnonCorrige = 0;
            int MautreInterne = 0;
            int MautreExterne = 0;

            int copen = 0;
            int ccorrige = 0;
            int creopen = 0;
            int cimpossibleReproduire = 0;
            int cimpossibleCorriger = 0;
            int cdoublon = 0;
            int cnonAnormalie = 0;
            int csuspendu = 0;
            int cnonCorrige = 0;
            int cautreInterne = 0;
            int cautreExterne = 0;

            bool boolmail = false;

            if (type_client == "Externe")
            {
                boolmail = true;
            }

            foreach (projects projet in un_projet.projects)
            {

                Issue issue_projet = callApi.getIssuesFromProject(projet.id).Result;

                foreach (issue iss in issue_projet.issues)
                {
                    if ((datedeb < iss.created_at & iss.created_at < datefi) || (datedeb.Equals(DateTime.MinValue)))
                    {
                        if ((!iss.reporters.email.Contains("celios") == boolmail) || (type_client == "Tous"))
                        {
                            issue += 1;

                            // L'issue est t'il interne ou externe ?
                            var issuetype = Localite(issueInterne, issueExterne, iss);
                            issueInterne = issuetype.Item1;
                            issueExterne = issuetype.Item2;
                            //La date de l'issue se situe entre la zone choisi ?
                            nbdate = Est_Dans_Les_Dates(datedeb, datefi, iss, nbdate);

                            //L'issue a t'elle redemandé une correction ?
                            nbrecorect = Recorrigé(iss, nbrecorect);

                            //L'issue a t'elle été résolu ?
                            var nonresolu = Résolu(iss, nonresoluInterne, nonresoluExterne);
                            nonresoluInterne = nonresolu.Item1;
                            nonresoluExterne = nonresolu.Item2;

                            if (iss.status.name == "closed")
                            {
                        
                                var typeresolution = Resolution(mopen,mcorrige,mreopen,mimpossibleReproduire,mimpossibleCorriger,mdoublon,mnonAnormalie,msuspendu,mnonCorrige,mautreInterne,mautreExterne, Mopen,Mcorrige,Mreopen, MimpossibleReproduire, MimpossibleCorriger, Mdoublon, MnonAnormalie, Msuspendu, MnonCorrige, MautreInterne, MautreExterne, copen,ccorrige,creopen, cimpossibleReproduire, cimpossibleCorriger, cdoublon, cnonAnormalie, csuspendu, cnonCorrige, cautreInterne, cautreExterne,iss);

                                mcorrige = typeresolution.Item1.Item1;
                                mimpossibleReproduire = typeresolution.Item1.Item3;
                                mimpossibleCorriger = typeresolution.Item2.Item1;
                                mdoublon = typeresolution.Item3.Item1;
                                mnonAnormalie = typeresolution.Item4.Item1;
                                msuspendu = typeresolution.Item5.Item1;
                                mnonCorrige = typeresolution.Item6.Item1;
                                mopen = typeresolution.Item7.Item1;
                                mreopen = typeresolution.Item7.Item4;

                                Mcorrige = typeresolution.Item1.Item2;
                                MimpossibleReproduire = typeresolution.Item1.Item5;
                                MimpossibleCorriger = typeresolution.Item2.Item2;
                                Mdoublon = typeresolution.Item3.Item2;
                                MnonAnormalie = typeresolution.Item4.Item2;
                                Msuspendu = typeresolution.Item5.Item2;
                                MnonCorrige = typeresolution.Item6.Item2;
                                Mopen = typeresolution.Item7.Item2;
                                Mreopen = typeresolution.Item7.Item5;

                                ccorrige = typeresolution.Item1.Item3;
                                cimpossibleReproduire = typeresolution.Item1.Item6;
                                cimpossibleCorriger = typeresolution.Item2.Item3;
                                cdoublon = typeresolution.Item3.Item3;
                                cnonAnormalie = typeresolution.Item4.Item3;
                                csuspendu = typeresolution.Item5.Item3;
                                cnonCorrige = typeresolution.Item6.Item3;
                                copen = typeresolution.Item7.Item3;
                                creopen = typeresolution.Item7.Item6;
                            }

                            var Diff = DateTime.Today - iss.created_at;
                            var Tolerance = new TimeSpan(2, 0, 36, 0); //+ 15h de tolérance = 7.2h sur une journée + 7.2h sur une journée + 0.6h sur la journée d'après
                            var Seuil = new TimeSpan(0, 0, 0, 0);

                            //Le projet a t'il été crée récement ?
                            if ((Tolerance - Diff) > Seuil && iss.status.name != "resolved" && iss.status.name != "closed")
                            {
                                // Détermine si un issue est mineur, majeur ou critique
                                var severite_retard = Severite_Retard(mretard, Mretard, cretard, cretardfini, Mretardfini, mretardfini, iss);
                                mretard = severite_retard.Item1;
                                Mretard = severite_retard.Item2;
                                cretard = severite_retard.Item3;
                                cretardfini = severite_retard.Item4;
                                Mretardfini = severite_retard.Item5;
                                mretardfini = severite_retard.Item6;
                            }

                            //Severite
                            var severite = Severite(smineur, mEvolution, mBug, mPlantage, mTMA, smajeur, MEvolution, MBug, MPlantage, MTMA, scritique, cEvolution, cBug, cPlantage, cTMA, iss);
                            smineur = severite.Item1.Item1;
                            mEvolution = severite.Item1.Item2;
                            mBug = severite.Item1.Item3;
                            mPlantage = severite.Item1.Item4;
                            mTMA = severite.Item1.Item5;
                            smajeur = severite.Item2.Item1;
                            MEvolution = severite.Item2.Item2;
                            MBug = severite.Item2.Item3;
                            MPlantage = severite.Item2.Item4;
                            MTMA = severite.Item2.Item5;
                            scritique = severite.Item3.Item1;
                            cEvolution = severite.Item3.Item2;
                            cBug = severite.Item3.Item3;
                            cPlantage = severite.Item3.Item4;
                            cTMA = severite.Item3.Item5;
                        }
                    }
                }
            }

    

            var nom_issue = new string[] { 
                "Nombre total d'issues dans la plage de date", "Nombre d'issues interne", "Nombre d'issues interne non résolus", "Nombre d'issues externe", 
                "Nombre d'issues externe non résolus", "Nombre d'issues total", "Nombre d'issues ayant demande une recorrection", "",
                "Nombre d'issues mineure", "  Dont résolus ouvert", "  Dont résolus corrigé","  Dont résolus réouvert", "  Dont résolus impossible à reproduire", "  Dont résolus impossible à corriger",
                "  Dont résolus doublon", "  Dont résolus pas d'anormalie", "  Dont résolus suspendus", "  Dont résolus ne sera pas corrigé", "",
                "Nombre d'issues majeure", "  Dont résolus ouvert", "  Dont résolus corrigé", "  Dont résolus réouvert", "  Dont résolus impossible à reproduire", "  Dont résolus impossible à corriger",
                "  Dont résolus doublon", "  Dont résolus pas d'anormalie", "  Dont résolus suspendus", "  Dont résolus ne sera pas corrigé","",
                "Nombre d'issues critique", "  Dont résolus ouvert","  Dont résolus corrigé", "  Dont résolus réouvert", "  Dont résolus impossible à reproduire", "  Dont résolus impossible à corriger",
                "  Dont résolus doublon", "  Dont résolus pas d'anormalie", "  Dont résolus suspendus", "  Dont résolus ne sera pas corrigé","",
                "Nombre d'issues mineure étant en retard", "Nombre d'issues majeure etant en retard", "Nombre d'issues critique etant en retard", "Nombre d'issues mineure ayant fini en retard",
                "Nombre d'issues majeure ayant fini en retard", "Nombre d'issues critique ayant fini en retard","",
                "Nombre d'issues mineur Evolution", "Nombre d'issues majeur Evolution", "Nombre d'issues critique Evolution", "",
                "Nombre d'issues mineur Bug", "Nombre d'issues majeur Bug", "Nombre d'issues critique Bug", "",
                "Nombre d'issues mineur Plantage", "Nombre d'issues majeur Plantage", "Nombre d'issues critique Plantage","",
                "Nombre d'issues mineur TMA", "Nombre d'issues majeur TMA", "Nombre d'issues critique TMA",""
            };
            var total_issue = new int[] { 
                nbdate, issueInterne, nonresoluInterne, issueExterne, nonresoluExterne, issue, nbrecorect, -1,
                smineur, mopen, mcorrige, mreopen, mimpossibleReproduire, mimpossibleCorriger, mdoublon, mnonAnormalie, msuspendu, mnonCorrige, -1,
                smajeur, Mopen, Mcorrige, Mreopen, MimpossibleReproduire, MimpossibleCorriger, Mdoublon, MnonAnormalie, Msuspendu, MnonCorrige,-1,
                scritique, copen, ccorrige, creopen, cimpossibleReproduire, cimpossibleCorriger, cdoublon, cnonAnormalie, csuspendu, cnonCorrige,-1,
                mretard, Mretard, cretard, mretardfini, Mretardfini, cretardfini, -1,
                mEvolution, MEvolution, cEvolution,-1,
                mBug, MBug, cBug, -1,
                mPlantage, MPlantage, cPlantage, -1,
                mTMA, MTMA, cTMA
            };

            var nbLignes = 0;

            if (est_detaille == false)
            {
                nbLignes = 41;
            }
            else
            {
                nbLignes = nom_issue.Length -1;
            }

            row = table.NewRow();
            row["Nom du projet"] = un_projet.projects[0].name;
            row["issue"] = "";
            row["total"] = DBNull.Value;
            table.Rows.Add(row);

            for (var i = 0; i < nbLignes; i++) {
                if (i == 0)
                {
                    if (!datedeb.Equals(DateTime.MinValue))
                    {
                        row = table.NewRow();
                        row["Nom du projet"] = "";
                        row["issue"] = nom_issue[i];
                        row["total"] = total_issue[i];
                        table.Rows.Add(row);
                    }
                } else
                {
                    row = table.NewRow();
                    row["Nom du projet"] = "";
                    row["issue"] = nom_issue[i];
                    if (total_issue[i] != -1)
                    {
                        row["total"] = total_issue[i];
                    }
                    else
                    {
                        row["total"] = DBNull.Value;
                    }
                    table.Rows.Add(row);
                }
            }

        }
        DataTable dt = table;

        #region Export Grid to CSV

        //Create the CSV file to which grid data will be exported.
        StreamWriter sw = new StreamWriter(strFilePath, false);


        //DataTable dt = m_dsProducts.Tables[0];
        int iColCount = dt.Columns.Count;

        for (int i = 0; i < iColCount; i++)
        {
            sw.Write(dt.Columns[i]);
            if (i < iColCount - 1)
            {
                sw.Write(";");
            }
        }
        sw.Write(sw.NewLine);

        //Now write all the rows.
        foreach (DataRow dr in dt.Rows)
        {
            for (int i = 0; i < iColCount; i++)
            {
                if (!Convert.IsDBNull(dr[i]))
                {
                    sw.Write(dr[i].ToString());
                }
                if (i < iColCount - 1)
                {
                    sw.Write(";");
                }
            }
            sw.Write(sw.NewLine);
        }
        
        ///
        sw.Close();
        #endregion Export Grid to CSV
 
    }

    /**
    * Méthode Est_Dans_Les_Dates
    *
    *      cette méthode permet de déterminer si la date du projet se situe dans la zone de date selectioné dans la vue 'général'
    *
    *
    * @param datedeb    Date minimum
    * @param datefi     Date maximum
    * @param iss        Une issue qu'on souhaite obtenir son mail
    *
    * Créé : 27/05/2022 Levanier
    */

    public int Est_Dans_Les_Dates(DateTime datedeb, DateTime datefi, issue iss, int nbdate)
    {

        if (datedeb < iss.created_at & iss.created_at < datefi)
        {

            nbdate += 1;
        }
        return nbdate;

    }
    public IActionResult Index()
    {
        return View();
    }

    /**
    * Méthode Localite
    *
    *      cette méthode permet de déterminer si un projet a été fait depuis Depuis Celios (interne) ou dans un autre endroit (externe) =
    *
    *
    * @param issueInterne     Nombre d'issue en Interne
    * @param issueExterne     Nombre d'issue en Externe
    * @param iss              Une issue qu'on souhaite obtenir son mail
    *
    * Note: Cherche si le mail du projet proviens de Celios
    * Créé : 27/05/2022 Levanier
    */
    public Tuple<int, int> Localite(int issueInterne, int issueExterne, issue iss)
    {

        if (iss.reporters.email.Contains("celios") == true)
        {
            issueInterne += 1;
        }
        else
        {
            issueExterne += 1;
        }
        return new Tuple<int, int>(issueInterne, issueExterne);
    }

    /**
    * Méthode Recorrigé
    *
    *      cette méthode permet de déterminer si l'issue à redemandé une recorrection
    *
    *
    * @param nbrecorect  Nombre d'issue nécésittant une recorrection
    * @param iss        Une issue qu'on souhaite obtenir son mail
    *
    * Note: Détermine si l'issue a été demandé plus d'une fois depuis l'historique
    * Créé : 27/05/2022 Levanier
    */
    public int Recorrigé(issue iss, int nbrecorect)
    {
        if (iss.histories.Count > 2)
        {
            nbrecorect = +1;
        }
        return nbrecorect;
    }

    /**
    * Méthode Résolu
    *
    *      cette méthode permet de déterminer si l'issue à été Résolu
    *
    *
    * @param nonresolu  Nombre d'issue qui ont été Résolu
    * @param iss        Une issue qu'on souhaite obtenir son mail
    *
    * Créé : 27/05/2022 Levanier
    */
    public Tuple<int, int> Résolu(issue iss, int nonresoluI, int nonresoluE)
    {
        if (iss.status.name != "resolved" && iss.status.name != "closed")
        {
            var nonresolu = Localite(nonresoluI, nonresoluE, iss);
            nonresoluI = nonresolu.Item1;
            nonresoluE = nonresolu.Item2;
        }
        return new Tuple<int, int>(nonresoluI, nonresoluE);
    }

    public Tuple<Tuple<int, int, int, int, int, int>, Tuple<int, int, int>, Tuple<int, int, int>, Tuple<int, int, int>, Tuple<int, int, int>, Tuple<int, int, int>, Tuple<int, int, int, int, int, int>> Resolution(int mopen,int mcorrige,int mreopen, int mimpossibleReproduire, int mimpossibleCorriger,int mdoublon, int mnonAnormalie, int msuspendu, int mnonCorrige, int mautreInterne, int mautreExterne, int Mopen, int Mcorrige,int Mreopen, int MimpossibleReproduire, int MimpossibleCorriger, int Mdoublon, int MnonAnormalie, int Msuspendu, int MnonCorrige, int MautreInterne, int MautreExterne, int copen, int ccorrige, int creopen, int cimpossibleReproduire, int cimpossibleCorriger, int cdoublon, int cnonAnormalie, int csuspendu, int cnonCorrige, int cautreInterne, int cautreExterne, issue iss)
    {

            switch (iss.resolutions.name)
            {
                case "fixed":
                switch (iss.severities.label)
                {
                    case "mineur":
                    case "texte":
                    case "cosmetique":
                        mcorrige += 1;
                        break;

                    case "majeur":
                        Mcorrige += 1;
                        break;

                    case "critique":
                    case "blocquant":
                    case "crash":
                        ccorrige += 1;
                        break;

                    default:
                        break;
                }
                break;


            case "unable to duplicate":
                switch (iss.severities.label)
                {
                    case "mineur":
                    case "texte":
                    case "cosmetique":
                        mimpossibleReproduire += 1;
                        break;

                    case "majeur":
                        MimpossibleReproduire += 1;
                        break;

                    case "critique":
                    case "blocquant":
                    case "crash":
                        cimpossibleReproduire += 1;
                        break;

                    default:
                        break;
                }
                break;

            case "not fixable":
                switch (iss.severities.label)
                {
                    case "mineur":
                    case "texte":
                    case "cosmetique":
                        mimpossibleCorriger += 1;
                        break;

                    case "majeur":
                        MimpossibleCorriger += 1;
                        break;

                    case "critique":
                    case "blocquant":
                    case "crash":
                        cimpossibleCorriger += 1;
                        break;

                    default:
                        break;
                }
                break;

            case "duplicate":
                switch (iss.severities.label)
                {
                    case "mineur":
                    case "texte":
                    case "cosmetique":
                        mdoublon += 1;
                        break;

                    case "majeur":
                        Mdoublon += 1;
                        break;

                    case "critique":
                    case "blocquant":
                    case "crash":
                        cdoublon += 1;
                        break;

                    default:
                        break;
                }
                break;

            case "not a bug":
                switch (iss.severities.label)
                {
                    case "mineur":
                    case "texte":
                    case "cosmetique":
                        mnonAnormalie += 1;
                        break;

                    case "majeur":
                        MnonAnormalie += 1;
                        break;

                    case "critique":
                    case "blocquant":
                    case "crash":
                        cnonAnormalie += 1;
                        break;

                    default:
                        break;
                }
                break;

            case "suspended":
                switch (iss.severities.label)
                {
                    case "mineur":
                    case "texte":
                    case "cosmetique":
                        msuspendu += 1;
                        break;

                    case "majeur":
                        Msuspendu += 1;
                        break;

                    case "critique":
                    case "blocquant":
                    case "crash":
                        csuspendu += 1;
                        break;

                    default:
                        break;
                }
                break;

            case "wont fix":
                switch (iss.severities.label)
                {
                    case "mineur":
                    case "texte":
                    case "cosmetique":
                        mnonCorrige += 1;
                        break;

                    case "majeur":
                        MnonCorrige += 1;
                        break;

                    case "critique":
                    case "blocquant":
                    case "crash":
                        cnonCorrige += 1;
                        break;

                    default:
                        break;
                }
                break;

            case "open":
                switch (iss.severities.label)
                {
                    case "mineur":
                    case "texte":
                    case "cosmetique":
                        mopen += 1;
                        break;

                    case "majeur":
                        Mopen += 1;
                        break;

                    case "critique":
                    case "blocquant":
                    case "crash":
                        copen += 1;
                        break;

                    default:
                        break;
                }
                break;

            case "reopen":
                switch (iss.severities.label)
                {
                    case "mineur":
                    case "texte":
                    case "cosmetique":
                        mreopen += 1;
                        break;

                    case "majeur":
                        Mreopen += 1;
                        break;

                    case "critique":
                    case "blocquant":
                    case "crash":
                        creopen += 1;
                        break;

                    default:
                        break;
                }
                break;

            default:
                if (iss.reporters.email.Contains("celios") == true)
                {
                    switch (iss.severities.label)
                    {
                        case "mineur":
                        case "texte":
                        case "cosmetique":
                            mautreInterne += 1;
                            break;

                        case "majeur":
                            MautreInterne += 1;
                            break;

                        case "critique":
                        case "blocquant":
                        case "crash":
                            cautreInterne += 1;
                            break;

                        default:
                            break;
                    }
                    break;
                } else
                {
                    switch (iss.severities.label)
                    {
                        case "mineur":
                        case "texte":
                        case "cosmetique":
                            mautreExterne += 1;
                            break;

                        case "majeur":
                            MautreExterne += 1;
                            break;

                        case "critique":
                        case "blocquant":
                        case "crash":
                            cautreExterne += 1;
                            break;

                        default:
                            break;
                    }
                    break;
                }
                    break;
            }
        
        Tuple<int, int, int, int, int, int> part1 = new Tuple<int, int, int, int, int, int>(mcorrige,Mcorrige,ccorrige, mimpossibleReproduire, MimpossibleReproduire, cimpossibleReproduire);
        Tuple<int, int, int> part2 = new Tuple<int, int, int>(mimpossibleCorriger, MimpossibleCorriger, cimpossibleCorriger);
        Tuple<int, int, int> part3 = new Tuple<int, int, int>(mdoublon, Mdoublon, cdoublon);
        Tuple<int, int, int> part4 = new Tuple<int, int, int>(mnonAnormalie, MnonAnormalie, cnonAnormalie);
        Tuple<int, int, int> part5 = new Tuple<int, int, int>(msuspendu, Msuspendu, csuspendu);
        Tuple<int, int, int> part6 = new Tuple<int, int, int>(mnonCorrige, MnonCorrige, cnonCorrige);
        Tuple<int, int,int,int,int,int> part7 = new Tuple<int, int, int, int, int, int>(mopen, Mopen, copen, mreopen, Mreopen, copen);

        
        return new Tuple<Tuple<int, int, int, int, int, int>, Tuple<int, int, int>, Tuple<int, int, int>, Tuple<int, int, int>, Tuple<int, int, int>, Tuple<int, int, int>, Tuple<int, int, int, int, int, int>>(part1, part2, part3, part4, part5, part6, part7);
    }
        /**
        * Méthode Severite_Retard
        *
        *      cette méthode permet de déterminer si un projet fini ou en retard (Non résolu depuis pluslieurs jours) est mineur, majeur ou critique
        *
        *
        * @param mretard,Mretard,cretard     Severite d'un issue en retard
        * @param cretardfini,Mretardfini,mretardfini     Severite d'un issue fini
        * @param iss              Une issue qu'on souhaite obtenir son mail
        *
        * Note: Cherche si le mail du projet proviens de Celios
        * Créé : 27/05/2022 Levanier
        */
        public Tuple<int, int, int, int, int, int> Severite_Retard(int mretard, int Mretard, int cretard, int cretardfini, int Mretardfini, int mretardfini, issue iss)
    {
        if (iss.status.name == "resolved")
        {
            switch (iss.severities.label)
            {
                case "mineur": case "texte": case "cosmetique":
                    mretardfini += 1;
                    break;

                case "majeur":
                    Mretardfini += 1;
                    break;

                case "critique": case "blocquant": case "crash":
                    cretardfini += 1;
                    break;

                default:
                    break;
            }
        }
        else
        {
            switch (iss.severities.label)
            {
                case "mineur":
                case "texte":
                case "cosmétique":
                    mretard += 1;
                    break;

                case "majeur":
                    Mretard += 1;
                    break;

                case "critique":
                case "bloquant":
                case "crash":
                    cretard += 1;
                    break;

                default:
                    break;
            }
        }
        return new Tuple<int, int, int, int, int, int>(mretard, Mretard, cretard, cretardfini, Mretardfini, mretardfini);
    }

    /**
    * Méthode Severite
    *
    *      cette méthode permet de déterminer le type d'un d'issue mineur, majeur ou critique
    *
    *
    * @param mretard,Mretard,cretard     Severite d'un issue en retard
    * @param cretardfini,Mretardfini,mretardfini     Severite d'un issue fini
    * @param iss              Une issue qu'on souhaite obtenir son mail
    *
    * Note: Cherche si le mail du projet proviens de Celios
    * Créé : 27/05/2022 Levanier
    */
    public Tuple<Tuple<int, int, int, int, int>, Tuple<int, int, int, int, int>, Tuple<int, int, int, int, int>> Severite(int smineur, int mEvolution, int mBug, int mPlantage, int mTMA, int smajeur, int MEvolution, int MBug, int MPlantage, int MTMA, int scritique, int cEvolution, int cBug, int cPlantage, int cTMA, issue iss)
    {
        if (iss.severities.label == "mineur" || iss.severities.label == "texte" || iss.severities.label == "cosmétique")
        {
            smineur += 1;
            switch (iss.categories.name)
            {
                case "Evolution":
                    mEvolution += 1;
                    break;

                case "Bug":
                    mBug += 1;
                    break;

                case "Plantage":
                    mPlantage += 1;
                    break;

                case "General":
                    mTMA += 1;
                    break;

                default:
                    break;
            }
        }
        if (iss.severities.label == "majeur")
        {
            smajeur += 1;
            switch (iss.categories.name)
            {
                case "Evolution":
                    MEvolution += 1;
                    break;

                case "Bug":
                    MBug += 1;
                    break;

                case "Plantage":
                    MPlantage += 1;
                    break;

                case "General":
                    MTMA += 1;
                    break;

                default:
                    break;
            }
        }
        if (iss.severities.label == "critique" || iss.severities.label == "bloquant" || iss.severities.label == "crash")
        {
            scritique += 1;
            switch (iss.categories.name)
            {
                case "Evolution":
                    cEvolution += 1;
                    break;

                case "Bug":
                    cBug += 1;
                    break;

                case "Plantage":
                    cPlantage += 1;
                    break;

                case "General":
                    cTMA += 1;
                    break;

                default:
                    break;
            }
        }
        Tuple<int, int, int, int, int> part1 = new Tuple<int, int, int, int, int>(smineur, mEvolution, mBug, mPlantage, mTMA);
        Tuple<int, int, int, int, int> part2 = new Tuple<int, int, int, int, int>(smajeur, MEvolution, MBug, MPlantage, MTMA);
        Tuple<int, int, int, int, int> part3 = new Tuple<int, int, int, int, int>(scritique, cEvolution, cBug, cPlantage, cTMA);

        return new Tuple<Tuple<int, int, int, int, int>, Tuple<int, int, int, int, int>, Tuple<int, int, int, int, int>>(part1, part2, part3);
    }

     public async Task<IActionResult> DownloadFile() {
        System.Threading.Thread.Sleep(2000);
        var memoryStream = new MemoryStream();
        var filePath = "";
        var fileName = "";
        bool err = true;
        while (err == true) 
        {
            if (Namefile.Contains("bilan_")){
                fileName = Namefile;
            } else {
                fileName = "bilan_" + Namefile + ".csv";
            }

            var fileModels = new FilesModels();
            IFileProvider provider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "./Historique"));

            foreach (var item in provider.GetDirectoryContents(""))
            {
                fileModels.files.Add(new FileModel()
                {
                    FileName = item.Name,
                    FilePath = item.PhysicalPath
                });
            }

            filePath = Path.Combine(Directory.GetCurrentDirectory(), "Historique", fileName);

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    await stream.CopyToAsync(memoryStream);
                    err = false;
                }

            }
            catch (Exception ex)
            {
                err = true;
            }
        }

        memoryStream.Position = 0;

        return File(memoryStream, "text/csv", Path.GetFileName(filePath));
    }

}