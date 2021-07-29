using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PUPPIGUI;
using PUPPIModel;
using PUPPICAD;
using System.Collections;
using System.IO;

//sample modules for outputting custom data
namespace customOutputModules
{
    //custom module: writes number,list of numbers or grid (arraylist of arraylists) to a file
    public class PUPPIWriteToCSV : PUPPIModule
    {
        public PUPPIWriteToCSV()
            : base()
        {
            name = "Write CSV";
            description = "Writes object,1D or 2D array/collections to a comma separated text file";
            //for output, we will display a message 
            outputs.Add("not-run");
            outputnames.Add("Result");
            //multiple types of variables will be accounted for
            inputnames.Add("To Write");
            inputs.Add(new PUPPIInParameter());
            //also the complete file path as string
            inputnames.Add("Output file path");
            inputs.Add(new PUPPIInParameter());
            //we will access inputs and outputs directly
            completeProcessOverride = true;
        }
        //file will get overwritten every time the program runs
        public override void process_usercode()
        {

            //if failure of any way, we report it
            //for instance, if not all inputs are connected, etc.
            //with completeprocessoverride enabled we need to do our own error checking
            try
            {
                if (inputs[0].module == null || inputs[1].module == null ) return;
                //get the path. we reference the connected output directly
                string fpath = inputs[1].module.outputs[inputs[1].outParIndex].ToString(); 
                if (!Directory.Exists (Path.GetDirectoryName(fpath)  )  )
                {
                    outputs[0] = "Invalid folder";
                    return;
                }
            

                ArrayList rows = new ArrayList();
                List<string> myrow = new List<string>();
                try
                {

                    string newcap = "";
                    
                        object os = inputs[0].module.outputs[inputs[0].outParIndex];
                        if (os.GetType().IsArray)
                        {
                            Array aos = os as Array;
                            rows = new ArrayList();
                            if (aos.Rank == 1)
                            {
                                int lb = aos.GetLowerBound(0);
                                int ub = aos.GetUpperBound(0);
                                myrow = new List<string>();
                                for (int ic = lb; ic <= ub; ic++)
                                {
                                    object myv = aos.GetValue(ic);
                                    myrow.Add(myv.ToString());
                                }
                                rows.Add(myrow);
                            }
                            else if (aos.Rank == 2)
                            {
                                int lb1 = aos.GetLowerBound(0);
                                int ub1 = aos.GetUpperBound(0);
                                int lb2 = aos.GetLowerBound(1);
                                int ub2 = aos.GetUpperBound(1);


                                for (int ic = lb1; ic <= ub1; ic++)
                                {
                                    myrow = new List<string>();
                                    for (int jc = lb2; jc <= ub2; jc++)
                                    {
                                        object myv = aos.GetValue(ic, jc);
                                        myrow.Add(myv.ToString());
                                    }
                                    rows.Add(myrow);
                                }
                            }
                            else
                            {
                                rows = new ArrayList();
                                myrow = new List<string>();
                                myrow.Add("Invalid array rank.");
                                rows.Add(myrow);
                            }
                        }
                        else
                        {
                            if (os is IEnumerable)
                            {
                                ArrayList ay = PUPPIModel.PUPPIModule.makeCollOrEnumIntoArrayList(os);
                                for (int ayi = 0; ayi < ay.Count; ayi++)
                                {
                                    List<string> thisrow = new List<string>();
                                    object osy = ay[ayi];
                                    if (osy is IEnumerable && osy.GetType()!=typeof(string) )
                                    {
                                        ArrayList ray = PUPPIModel.PUPPIModule.makeCollOrEnumIntoArrayList(osy);
                                        for (int rayi = 0; rayi < ray.Count; rayi++)
                                        {
                                            thisrow.Add(ray[rayi].ToString());
                                        }
                                    }
                                    else
                                    {
                                        thisrow.Add(osy.ToString());
                                    }

                                    rows.Add(thisrow);
                                }

                            }
                            else
                            {
                                List<string> thisrow = new List<string>();
                                thisrow.Add(os.ToString());
                                rows.Add(thisrow);

                            }
                        }

                    
                }
                catch
                {
                    outputs[0] = "error reading data";
                    return;

                }

                if (rows.Count ==0 )
                {
                    outputs[0] = "empty";
                    return;
                }
                string[] allrows = new string[rows.Count ];
                for (int rc = 0; rc < rows.Count;rc++ )
                {
                    string thisrow = "";
                    myrow = rows[rc] as List<string>;
                    for (int cc=0;cc<myrow.Count;cc++  )
                    {
                        thisrow = thisrow + myrow[cc];
                        if (cc != myrow.Count - 1) thisrow += ",";
                    }
                    allrows[rc] = thisrow;  
                }
                System.IO.File.WriteAllLines(fpath, allrows);
                    

                outputs[0] = "success";
            }
            catch
            {
                outputs[0] = "error";
                return;
            }





            return;
        }


    }

    public class PUPPIBMPToFile : PUPPIModule
    {

        public PUPPIBMPToFile ():base()
        {

            name = "Bitmap to File";
            description = "Saves a System.Drawing.Bitmap object to a file";
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("BMP");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("File path");
            outputs.Add(null);
            outputnames.Add("Saved file path"); 
    }
        public override void process_usercode()
        {
            try
            {

                System.Drawing.Bitmap myBmp = usercodeinputs[0] as System.Drawing.Bitmap;
                string fo = usercodeinputs[1].ToString();
                myBmp.Save(fo);
                usercodeoutputs[0] = fo;
            }
            catch
            {
                usercodeoutputs[0] = "error";
            }
        }
    }
}
