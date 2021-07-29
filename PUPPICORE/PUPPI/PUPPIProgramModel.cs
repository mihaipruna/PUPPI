using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PUPPIModel;
using System.Xml;
using PUPPIDEBUG;
namespace PUPPIAbstraction
{
    public class PUPPIProgramModel
    {
        public Dictionary<int, PUPPIModel.PUPPIModule> logicalNodes;

        public int maxGUID = 0;
        public PUPPIProgramModel()
        {
            //add check
            logicalNodes = new Dictionary<int, PUPPIModel.PUPPIModule>();
            maxGUID = 0;
        }

        public static string writeToXML(Dictionary<int, PUPPIModel.PUPPIModule> logicalNodes)
        {
            string xmldata = "";
            try
            {
                XmlDocument doc = new XmlDocument();

                XmlNode programModelElem;

                programModelElem = doc.CreateNode(XmlNodeType.Element, "programModel", "");
                //sorta disregarding dictionary part
                foreach (PUPPIModule writeModule in logicalNodes.Values)
                {


                    if (writeModule == null) continue;
                    //each node
                    XmlNode nodeelem = doc.CreateNode(XmlNodeType.Element, "logicalNode", "");
                    XmlNode guideleme = doc.CreateNode(XmlNodeType.Element, "guid", "");
                    guideleme.InnerText = writeModule.GUID.ToString();
                    nodeelem.AppendChild(guideleme);

                    XmlNode PUPPImodelem = doc.CreateNode(XmlNodeType.Element, "PUPPImodule", "");

                    PUPPImodelem.InnerText = writeModule.GetType().ToString();

                    nodeelem.AppendChild(PUPPImodelem);
                    
                    //cleaned caption
                    XmlNode ccelem = doc.CreateNode(XmlNodeType.Element, "cleancap", "");

                    ccelem.InnerText = writeModule.cleancap;

                    nodeelem.AppendChild(ccelem);
                    
                    
                    //save custom node data

                    string s = writeModule.saveSettings();
                    
                        XmlNode cs = doc.CreateNode(XmlNodeType.Element, "savedSettings", "");
                        cs.InnerText = s;
                        nodeelem.AppendChild(cs);
                 


                    //array level - written after module
                    XmlNode ll = doc.CreateNode(XmlNodeType.Element, "listProcessing", "");
                    ll.InnerText = writeModule.listprocess.ToString();
                    nodeelem.AppendChild(ll);

                    ll = doc.CreateNode(XmlNodeType.Element, "listElementCount", "");
                    ll.InnerText = writeModule.countListMode.ToString();
                    nodeelem.AppendChild(ll);

                    for (int inc = 0; inc < writeModule.inputs.Count; inc++)
                    {
                        //write inputs
                        PUPPIInParameter ppi = writeModule.inputs[inc];
                        if (ppi == null) continue;
                        if (ppi.module == null) continue;
                        XmlNode conneleme = doc.CreateNode(XmlNodeType.Element, "connector", "");


                        // <inputindex>1</inputindex>
                        //  <nodeguid>3</nodeguid>
                        //<outputindex>2</outputindex>
                        XmlNode inieleme = doc.CreateNode(XmlNodeType.Element, "inputindex", "");
                        inieleme.InnerText = inc.ToString();
                        conneleme.AppendChild(inieleme);

                        XmlNode ininodeleme = doc.CreateNode(XmlNodeType.Element, "moduleguid", "");
                        if (ppi.module != null) { ininodeleme.InnerText = ppi.module.GUID.ToString(); }
                        else
                        {
                            ininodeleme.InnerText = "";
                        }
                        conneleme.AppendChild(ininodeleme);

                        XmlNode outpareleme = doc.CreateNode(XmlNodeType.Element, "outputindex", "");
                        outpareleme.InnerText = ppi.outParIndex.ToString();
                        conneleme.AppendChild(outpareleme);

                        nodeelem.AppendChild(conneleme);
                    }
                    //if no inputs, write output values if possible
                    //because otherwise the values would come from inputs
                    if (writeModule.inputs.Count == 0)
                    {
                        for (int outs = 0; outs < writeModule.outputs.Count; outs++)
                        {

                            if (writeModule.outputs[outs] == null) continue;
                            XmlNode outeleme = doc.CreateNode(XmlNodeType.Element, "output", "");
                            //index
                            XmlNode outindeleme = doc.CreateNode(XmlNodeType.Element, "outputindex", "");
                            outindeleme.InnerText = outs.ToString();
                            outeleme.AppendChild(outindeleme);
                            //this is for single value
                            if (writeModule.listprocess == false)
                            {

                                //arraylists need to be addressed for stuff such as grid inputs
                                //value
                                XmlNode outvaleleme = doc.CreateNode(XmlNodeType.Element, "outputvalue", "");
                                if (writeModule.outputs[outs].GetType() == typeof(ArrayList))
                                {
                                    outvaleleme.InnerText = PUPPIGUI.PUPPICanvas.writearraytostring(writeModule.outputs[outs] as ArrayList);
                                }
                                else
                                {

                                    try
                                    {
                                        outvaleleme.InnerText = writeModule.outputs[outs].ToString();
                                    }
                                    catch (Exception exy)
                                    {
                                        if (PUPPIDebugger.debugenabled)
                                        {
                                            PUPPIDebugger.log("Output write error: " + exy.ToString());
                                        }
                                        return null;
                                    }
                                }
                                outeleme.AppendChild(outvaleleme);
                            }
                            else
                            {

                                try
                                {
                                    ArrayList myovals = writeModule.outputs[outs] as ArrayList;
                                    for (int ooc = 0; ooc < myovals.Count; ooc++)
                                    {
                                        //value
                                        XmlNode outvaleleme = doc.CreateNode(XmlNodeType.Element, "outputvalue", "");
                                        outvaleleme.InnerText = myovals[ooc].ToString();
                                        outeleme.AppendChild(outvaleleme);
                                    }

                                }
                                catch(Exception exy)
                                {
                                    if (PUPPIDebugger.debugenabled)
                                    {
                                        PUPPIDebugger.log("Output write error: "+ exy.ToString());
                                    }
                                    return null;
                                }
                            }

                            nodeelem.AppendChild(outeleme);
                        }
                    }
                    //input settings
                    else
                    {
                        for (int ints = 0; ints < writeModule.inputs.Count; ints++)
                        {

                            if (writeModule.inputs[ints] == null) continue;
                            XmlNode inteleme = doc.CreateNode(XmlNodeType.Element, "input", "");
                            //index
                            XmlNode intindeleme = doc.CreateNode(XmlNodeType.Element, "inputindex", "");
                            intindeleme.InnerText = ints.ToString();
                            inteleme.AppendChild(intindeleme);
                            //autolist
                            XmlNode outvaleleme = doc.CreateNode(XmlNodeType.Element, "autoListMode", "");
                            outvaleleme.InnerText = writeModule.inputs[ints].inputAutomaticListMode.ToString();
                            inteleme.AppendChild(outvaleleme);
                            nodeelem.AppendChild(inteleme);
                        }
                    }
                    programModelElem.AppendChild(nodeelem);

                }
                doc.AppendChild(programModelElem);
                //save to string
                xmldata = doc.OuterXml;
            }
            catch(Exception exy)
            {
                if (PUPPIDebugger.debugenabled)
                {
                    PUPPIDebugger.log("XML save error " + exy.ToString());
                }
                return null;
            }

            return xmldata;
        }

        public static Dictionary<int, PUPPIModel.PUPPIModule> readFromXML(string XMLdata, int GUIDshift = 0)
        {

            Dictionary<int, PUPPIModel.PUPPIModule> newLogicalNodes = new Dictionary<int, PUPPIModel.PUPPIModule>();
            try
            {
                XmlDocument doc = new XmlDocument();
                try
                {

                    doc.LoadXml(XMLdata);

                }
                catch (Exception exy)
                {
                    if (PUPPIDebugger.debugenabled)
                    {
                        PUPPIDebugger.log("Load XML PUPPIProgramModel Error: " + XMLdata + " " + exy.ToString());
                    }
                    return null;
                }

                //first pass, load modules
                XmlNodeList nodes = doc.DocumentElement.SelectNodes("/programModel/logicalNode");

                foreach (XmlNode mynode in nodes)
                {
                    try
                    {


                        //module pass
                        PUPPIModule mm = null;


                        XmlNode childnode = mynode["PUPPImodule"];
                        //get the class by name
                        //from all loaded assemblies

                        Type tm = null;
                        string tName = childnode.InnerText;
                        tm = PUPPIModel.AutomaticPUPPImodulesCreator.findTypeByName(tName);

                        if (tm == null)
                        {
                            throw new Exception("Type " + tName + " not found");
                        }

                        object pm1 = System.Activator.CreateInstance(tm);
                        mm = pm1 as PUPPIModule;
                        if (mm == null) throw new Exception("nullmodule");
                        childnode = mynode["guid"];
                        mm.GUID = Convert.ToInt32(childnode.InnerText);
                        if (newLogicalNodes.ContainsKey(mm.GUID)) throw new Exception("Duplicate GUID in dictionary " + mm.GUID.ToString());

                        //load saved settings
                        string ss = "";
                        childnode = mynode["savedSettings"];

                        ss = childnode.InnerText;

                        try
                        {
                            childnode = mynode["cleancap"];

                            mm.cleancap = childnode.InnerText;
                        }
                        catch
                        {
                            mm.cleancap = "";
                        }

                        if (ss != "") mm.initOnLoad(ss);
                        mm.GUID += GUIDshift;
                        childnode = mynode["listProcessing"];
                        mm.listprocess = Convert.ToBoolean(childnode.InnerText);

                        newLogicalNodes.Add(mm.GUID, mm);
                    }


                    catch (Exception exy)
                    {
                        if (PUPPIDebugger.debugenabled)
                        {
                            PUPPIDebugger.log("XML Logical Node Instantiation error: " + mynode.InnerText + " " + exy.ToString());
                        }
                      //  return null;
                    }
                }

                //second pass, connect modules
                nodes = doc.DocumentElement.SelectNodes("/programModel/logicalNode");
                foreach (XmlNode mynode in nodes)
                {

                    //guid has to be the first item
                    PUPPIModule mm = newLogicalNodes[(Convert.ToInt32(mynode["guid"].InnerText) + GUIDshift)];

                   
                    //XmlNodeList childnodes = mynode.SelectNodes("//connector"); ;
                    //foreach (XmlNode childnode in childnodes)
                    //{
                    //    //for an input conenctor we need to know which input on the current node
                    //    //the node it connects to and which output on that node
                    //    // <inputindex>1</inputindex>
                    //    //  <nodeguid>3</nodeguid>
                    //    //<outputindex>2</outputindex>
                    //    try
                    //    {
                    //        XmlNode ii = childnode.ChildNodes[0];
                    //        int inputindex = Convert.ToInt32(ii.InnerText);
                    //        ii = childnode.ChildNodes[1];

                    //        PUPPIModule sourcemodule = newLogicalNodes[(Convert.ToInt32(ii.InnerText) + GUIDshift)];

                    //        ii = childnode.ChildNodes[2];
                    //        int outparindex = Convert.ToInt32(ii.InnerText);

                    //        mm.inputs[inputindex].module = sourcemodule;
                    //        mm.inputs[inputindex].outParIndex = outparindex;
                    //        //testModule.logical_representation.inputs[inputindex].isoptional= isoptional;
                    //        //set up so that when this changes we get a notification
                    //        mm.inputs[inputindex].sourcenodeevent(sourcemodule);
                    //        //conenct the logic to appropriate events to pass it along to the destination
                    //        mm.connectevent(mm.inputs[inputindex]);
                    //    }
                    //    catch (Exception exy)
                    //    {
                    //        if (PUPPIDebugger.debugenabled)
                    //        {
                    //            PUPPIDebugger.log("XML Logical Node Connection  error: " + mynode.InnerText + " " + exy.ToString());
                    //        }
                    //        return null;
                    //    }
                    //}
                    //childnodes = mynode.SelectNodes("//output");
                    //foreach (XmlNode childnode in childnodes)
                    //{
                    //    //childnode = mynode["output"];


                    //    try
                    //    {
                    //        XmlNode oi = childnode.ChildNodes[0];
                    //        int outputindex = Convert.ToInt32(oi.InnerText);
                    //        //for single entry
                    //        if (!mm.listprocess)
                    //        {
                    //            XmlNode ov = childnode.ChildNodes[1];
                    //            // but we can still have lists and grids this way
                    //            if (mm.outputs[outputindex].GetType() == typeof(ArrayList))
                    //            {
                    //                mm.outputs[outputindex] = PUPPIGUI.PUPPICanvas.readALfromXMLstring(ov.InnerText);
                    //            }
                    //            else
                    //            {
                    //                mm.outputs[outputindex] = Convert.ChangeType(ov.InnerText, mm.outputs[outputindex].GetType());
                    //            }
                    //        }
                    //        else
                    //        {
                    //            int numberelems = childnode.ChildNodes.Count - 1;
                    //            //needed for correct display
                    //            mm.countListMode = numberelems;
                    //            //get the output type from default
                    //            Type otype = mm.outputs[outputindex].GetType();
                    //            ArrayList olist = new ArrayList();
                    //            for (int olcount = 1; olcount <= numberelems; olcount++)
                    //            {
                    //                XmlNode ov = childnode.ChildNodes[olcount];
                    //                olist.Add(Convert.ChangeType(ov.InnerText, otype));
                    //            }
                    //            mm.outputs[outputindex] = olist;

                    //        }
                    //    }
                    //    catch (Exception exy)
                    //    {
                    //        if (PUPPIDebugger.debugenabled)
                    //        {
                    //            PUPPIDebugger.log("XML Logical Node Connection  Output error: " + mynode.InnerText + " " + exy.ToString());
                    //        }
                    //        return null;
                    //    }
                    //}
                    ////auto list
                    //childnodes = mynode.SelectNodes("//input");
                    //foreach (XmlNode childnode in childnodes)
                    //{
                    //    //childnode = mynode["input"];
                    //    XmlNode iii = childnode.ChildNodes[0];

                    //    int inputindexi = Convert.ToInt32(iii.InnerText);
                    //    XmlNode iv = childnode.ChildNodes[1];
                    //    mm.inputs[inputindexi].inputAutomaticListMode = Convert.ToBoolean(iv.InnerText);
                    //}

                    XmlNodeList childnodes = mynode.ChildNodes;
                    foreach (XmlNode childnode in childnodes)
                    {
                        //XmlNodeList childnodes = mynode.SelectNodes("//connector"); ;
                        //for an input conenctor we need to know which input on the current node
                        //the node it connects to and which output on that node
                        // <inputindex>1</inputindex>
                        //  <nodeguid>3</nodeguid>
                        //<outputindex>2</outputindex>
                        if (childnode.Name == "connector")
                        {
                            try
                            {
                                XmlNode ii = childnode.ChildNodes[0];
                                int inputindex = Convert.ToInt32(ii.InnerText);
                                ii = childnode.ChildNodes[1];

                                PUPPIModule sourcemodule = newLogicalNodes[(Convert.ToInt32(ii.InnerText) + GUIDshift)];

                                ii = childnode.ChildNodes[2];
                                int outparindex = Convert.ToInt32(ii.InnerText);

                                mm.inputs[inputindex].module = sourcemodule;
                                mm.inputs[inputindex].outParIndex = outparindex;
                                //testModule.logical_representation.inputs[inputindex].isoptional= isoptional;
                                //set up so that when this changes we get a notification
                                mm.inputs[inputindex].sourcenodeevent(sourcemodule);
                                //conenct the logic to appropriate events to pass it along to the destination
                                mm.connectevent(mm.inputs[inputindex]);
                            }
                            catch (Exception exy)
                            {
                                if (PUPPIDebugger.debugenabled)
                                {
                                    PUPPIDebugger.log("XML Logical Node Connection  error: " + mynode.InnerText + " " + exy.ToString());
                                }
                                //return null;
                            }
                        }

                        if (childnode.Name == "output")
                        {

                            try
                            {
                                XmlNode oi = childnode.ChildNodes[0];
                                int outputindex = Convert.ToInt32(oi.InnerText);
                                //for single entry
                                if (!mm.listprocess)
                                {
                                    XmlNode ov = childnode.ChildNodes[1];
                                    // but we can still have lists and grids this way
                                    if (mm.outputs[outputindex].GetType() == typeof(ArrayList))
                                    {
                                        mm.outputs[outputindex] = PUPPIGUI.PUPPICanvas.readALfromXMLstring(ov.InnerText);
                                    }
                                    else
                                    {
                                        mm.outputs[outputindex] = Convert.ChangeType(ov.InnerText, mm.outputs[outputindex].GetType());
                                    }
                                }
                                else
                                {
                                    int numberelems = childnode.ChildNodes.Count - 1;
                                    //needed for correct display
                                    mm.countListMode = numberelems;
                                    //get the output type from default
                                    Type otype = mm.outputs[outputindex].GetType();
                                    ArrayList olist = new ArrayList();
                                    for (int olcount = 1; olcount <= numberelems; olcount++)
                                    {
                                        XmlNode ov = childnode.ChildNodes[olcount];
                                        olist.Add(Convert.ChangeType(ov.InnerText, otype));
                                    }
                                    mm.outputs[outputindex] = olist;

                                }
                            }
                            catch (Exception exy)
                            {
                                if (PUPPIDebugger.debugenabled)
                                {
                                    PUPPIDebugger.log("XML Logical Node Connection  Output error: " + mynode.InnerText + " " + exy.ToString());
                                }
                                //return null;
                            }
                        }
                        //auto list
                        if (childnode.Name == "input")
                        {
                            //childnode = mynode["input"];
                            XmlNode iii = childnode.ChildNodes[0];

                            int inputindexi = Convert.ToInt32(iii.InnerText);
                            XmlNode iv = childnode.ChildNodes[1];
                            mm.inputs[inputindexi].inputAutomaticListMode = Convert.ToBoolean(iv.InnerText);
                        }
                    }

                }
            }
            catch(Exception exy)
            {
                if (PUPPIDebugger.debugenabled)
                {
                    PUPPIDebugger.log("XML read error: " + XMLdata + " " + exy.ToString());
                }
                return null;
            }
            return newLogicalNodes;
        }



        public  bool addLogicalNode(PUPPIModel.PUPPIModule myModule, out int newLogicalGUID)
        {
            newLogicalGUID = 0;
            if (myModule == null) return false;
            while (logicalNodes.ContainsKey(maxGUID))
            {
                maxGUID++;
            }
            logicalNodes.Add(maxGUID, myModule);
            myModule.GUID = maxGUID;
            newLogicalGUID = maxGUID;
            return true;

        }
        public bool connectLogicalNodes(int sourceLogicalNodeGUID, int srcOutputIndex, int destLogicalNodeGUID, int dstInputIndex)
        {
            bool rv = true;
            PUPPIModel.PUPPIModule ps;
            PUPPIModel.PUPPIModule pd;
            bool rs = logicalNodes.TryGetValue(sourceLogicalNodeGUID, out ps);
            bool rd = logicalNodes.TryGetValue(destLogicalNodeGUID, out pd);
            if (!rs || !rd || ps == null || pd == null) return false;
            if (srcOutputIndex < 0 || srcOutputIndex >= ps.outputs.Count || dstInputIndex < 0 || dstInputIndex >= pd.inputs.Count) return false;

            //already filled number max outgoing connections
            //if (ps.maxOutgoingConnections >= 0 && sn.vizoutputs[outPindex].outboundconns.Count == sn.logical_representation.maxOutgoingConnections) return false;

            //    //create an input parameter and hook it up to the destination node
            PUPPIInParameter pinp = new PUPPIInParameter();
            pinp.getinputfrom(ps, srcOutputIndex);


            bool connected = pd.connect_input(pinp, dstInputIndex, false);
            if (connected == false) return false;

            pd.doIprocess();
            return true;
        }

        public bool disconnectLogicalNodeInput(int logicalNodeGUID, int logicalNodeInputIndex)
        {
            PUPPIModel.PUPPIModule ps;
            bool rs = logicalNodes.TryGetValue(logicalNodeGUID, out ps);
            if (!rs) return false;
            if (logicalNodeInputIndex < 0 || logicalNodeInputIndex >= ps.outputs.Count) return false;
            bool disco = ps.disconnect_input(logicalNodeInputIndex);
            return disco;
        }

        public int getLogicalNodeUpstream(int logicalNodeGUID, int logicalNodeInputIndex)
        {
            PUPPIModel.PUPPIModule ps;
            bool rs = logicalNodes.TryGetValue(logicalNodeGUID, out ps);
            if (!rs) return -1;
            if (logicalNodeInputIndex < 0 || logicalNodeInputIndex >= ps.outputs.Count) return -1;
            if (ps.inputs[logicalNodeInputIndex] == null) return -1;
            if (ps.inputs[logicalNodeInputIndex].module == null) return -1;
            return ps.inputs[logicalNodeInputIndex].module.GUID;

        }

        public int getLogicalNodeGUID(PUPPIModule module)
        {
            if (module == null) return -1;
            foreach (KeyValuePair<int, PUPPIModule> p in logicalNodes)
            {
                if (p.Value == module) return p.Key;
            }
            return -1;
        }

        public List<int> getLogicalNodesDownstream(int logicalNodeGUID, int logicalNodeOutputIndex)
        {
            List<int> l = new List<int>();
            PUPPIModel.PUPPIModule ps;
            bool rs = logicalNodes.TryGetValue(logicalNodeGUID, out ps);
            if (!rs) return l;
            foreach (KeyValuePair<int, PUPPIModule> p in logicalNodes)
            {
                if (p.Key != logicalNodeGUID)
                {
                    PUPPIModule pp = p.Value;
                    if (pp != null)
                    {
                        for (int i = 0; i < pp.inputs.Count; i++)
                        {
                            if (pp.inputs[i].module != null)
                            {
                                if (getLogicalNodeGUID(pp.inputs[i].module) == logicalNodeGUID && pp.inputs[i].outParIndex == logicalNodeOutputIndex)
                                {
                                    l.Add(p.Key);
                                }
                            }
                        }
                    }
                }
            }

            return l;
        }




    }
}
