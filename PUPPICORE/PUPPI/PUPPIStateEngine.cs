using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace PUPPI
{
    /// <summary>
    /// Class to facilitate the creation  and operation of state engines which can access specified classes and methods
    /// </summary>
    public class PUPPIStateEngine
    {
        internal static string Fexeunc(List<object> exeClasses,string typeName, string methodNmae, List<string> arguments)
        {
            string res = "";
            object fnd = null;
            foreach (object oo in exeClasses)
            {
                if (oo.GetType().ToString().ToLower().Contains(typeName.ToLower()))
                {
                    fnd = oo;
                    break;
                }
            }
            if (fnd == null) return "Class not found";
            Type ctype = fnd.GetType();
            foreach (MethodInfo mao in ctype.GetMethods())
            {
                if (mao.Name.ToLower().Contains(methodNmae.ToLower()))
                {
                    ParameterInfo[] pinfo = mao.GetParameters();
                    if (pinfo.Length == arguments.Count)
                    {
                        //found

                        object[] paramvals = new object[pinfo.Length];
                        for (int pc = 0; pc < pinfo.Length; pc++)
                        {
                            Type ptype = pinfo[pc].ParameterType;

                            //initialize param if input
                            Type ttype = ptype as Type;
                            if (pinfo[pc].IsOut == false)
                            {

                                Type tttt = ttype as Type;
                                //will catch immutable error
                                try
                                {
                                    paramvals[pc] = System.Activator.CreateInstance(ttype as Type);
                                }
                                catch
                                {
                                    paramvals[pc] = null;
                                }
                                try
                                {
                                    paramvals[pc] = Convert.ChangeType(arguments[pc], tttt);
                                }
                                catch
                                {
                                    return "failed to convert argument " + arguments[pc] + " to type " + tttt.ToString();
                                }
                            }
                            else
                            {

                                try
                                {
                                    paramvals[pc] = System.Activator.CreateInstance(ttype as Type);
                                }
                                catch (Exception exy)
                                {
                                    paramvals[pc] = null;
                                }
                            }
                        }

                        //method
                        object result = null;
                        object cco = null;
                        MethodInfo mthod = mao;
                        if (mthod.ReturnType != typeof(void))
                        {




                            result = mthod.Invoke(fnd, paramvals);




                        }
                        else
                        {

                            mthod.Invoke(fnd, paramvals);
                            result = "ran void method";


                        }
                        return result.ToString();
                    }
                }
            }
            return "Method not found";
        }
        /// <summary>
        /// List of objects whose methods are executed
        /// </summary>
        public List<object> exeClasses { get; set; }
        /// <summary>
        /// current state, which determines which method is executed 
        /// </summary>
        public int currentState { get; private set; }
        /// <summary>
        /// Adds information to create a new state which will execute a method from exeClasses with specified arguments supplied as strings
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="methodName"></param>
        /// <param name="argumentValues"></param>
        public void AddStateExecution(string objectName,string methodName,List<string>argumentValues)
        {
            on.Add(objectName);
            mn.Add(methodName);
            avs.Add(new List<string>(argumentValues));
            allstates++;
            if (currentState == -1) currentState++;
        }
        /// <summary>
        /// Skips state without executing
        /// </summary>
        /// <returns></returns>
        public bool SkipState()
        {
            if (allstates == 0) return false;
            if (currentState == allstates - 1) return false;
            currentState++;
            return true;
        }
        /// <summary>
        /// Goes back state without executing
        /// </summary>
        /// <returns></returns>
        public bool BackState()
        {
            if (allstates == 0) return false;
            if (currentState ==0) return false;
            
            currentState--;
            return true;
        }
        /// <summary>
        /// Executes state, increases state counter and return method execution result as string
        /// </summary>
        /// <returns></returns>
        public string ExecuteState()
        {
            if (currentState == allstates - 1) return "Reached last state";
            if (allstates==0) return "No state";
            string res = "not exec";
            try
            {
                res = Fexeunc(exeClasses, on[currentState], mn[currentState], avs[currentState]);
            }
            catch
            {
                res = "error";
            }
            currentState++;
            return res;
            
        }


        int allstates;
        List<string> on;
        List<string> mn;
        List<List<string>> avs;


        public PUPPIStateEngine()
        {
            exeClasses = new List<object>();
            on = new List<string>();
            mn = new List<string>();
            avs = new List<List<string>>();
            allstates = 0;
            currentState = -1;
        }
    }
}
