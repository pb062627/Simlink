using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HDF5DotNet;
using System.Data;
using System.IO;


namespace SIM_API_LINKS
{
    public class hdf5_wrap
    {//

        //HDF wrapper for getting generic data arrays to/from HDF5
        //to be compatible with SimLink and Hydra
        //General file architecture
        //1 HDF5 file per scenario- this reduces collisions between read / write
        // Group: top level group codes in a link to the element data (e.g tblResultTS_FK for SimLink)
        // Dataset name: an indicator of the dataclass; for now just use 1, but different TS operations will have a tlkpDictionary operation defined here. not all will be preserved in file

        //many, many options exist for 'optimizing' performance- chunking, grouping, etc... as this wrapper evolves we can improve the options

        #region OBJECT PROPERTIES

        // Define object properties - these are set for any hdf5_wrap object.
            public H5FileOrGroupId _h5FileOrGroupID;
            public H5LocId _h5loc;
            public H5GroupId _h5g;
            public H5DataSetId _h5d_Dataset;
            public H5DataSpaceId _h5d_Space;
            public H5DataTypeId _h5d_Type;
            public H5DataTypeId _h5d_TypeOut;
            public double[,] _h5d_DataArray_Double;
            public long[] _lH5_dim;
            public int _n_H5rank = -1;
            public int _n_H5_DATA_ARRAY_LENGTH = -1;
            public int _n_H5_DATA_ARRAY_WIDTH = -1;
            public string _sHDF_FileName = "NOTHING";
            public bool _bHDF_IsOpen = false;
            public H5F.OpenMode _h5FOpenMode;
            public bool _bOutputArrayFormat = false;        // met 10/10/17:  if true, export as 2D array to the hdf5.
            public const string _sGroupName_ArrayFormat = "TS_OUT";     // use a stanard group name in this case
  

            # endregion

            #region Utilities
            public string RMV_FixFilename(string sIncoming)
            {
                return sIncoming.Replace(":", "_").Replace(",", "_").Replace("!", "_").Replace("*", "_").Replace(" ", "_").Replace("/", "_").Replace("#", "_").Replace(" ", "$").Replace("%", "_").Replace("&", "_").Replace("(", "_").Replace(")", "_").Replace("^", "_");
            }

        //make sure these don't hold over.

        //todo : parameterize to provide greater control...
            private void ClearObjectVars()
            {
                _h5FileOrGroupID = null;
                _h5loc = null;
              _h5g= null;
              _h5d_Dataset= null;
              _h5d_Space = null;
              _h5d_Type = null;
              _h5d_TypeOut = null;
              _h5d_DataArray_Double = null;
              _lH5_dim = null;
              _n_H5rank = -1;
              _n_H5_DATA_ARRAY_LENGTH = -1;
              _n_H5_DATA_ARRAY_WIDTH = -1;

            }

            #endregion


            public bool hdfOpen(string sFileName, bool bIsReadOnly = true, bool bCreateIfNoExist = true)
            {
                bool bIsOpen = true;

                if (hdfCheckOrCreateH5(sFileName, bCreateIfNoExist))
                {

                    if (bIsReadOnly){
                        _h5FileOrGroupID = H5F.open(sFileName, H5F.OpenMode.ACC_RDONLY);
                        _h5FOpenMode = H5F.OpenMode.ACC_RDONLY;
                    }
                    else{
                        _h5FileOrGroupID = H5F.open(sFileName, H5F.OpenMode.ACC_RDWR);
                        _h5FOpenMode = H5F.OpenMode.ACC_RDWR;
                    }
                    _bHDF_IsOpen = true;
                    _sHDF_FileName = sFileName;
                }
                else
                {
                    _bHDF_IsOpen = false;
                    bIsOpen = false;
                }
                return bIsOpen;
            }

            #region READ_HDF5
            
        
        
        
            public double[,] hdfGetDataSeries(string sGroupName, string sDataSetName, bool bIsReadOnly = true, bool bWriteFails=false)
            {
                //met 9/11/14: set this to null because it was being left over from the previou
                //don't like this as a global. consider fixing.
                _h5d_DataArray_Double = null;   // new double[,];
                if (this._bHDF_IsOpen)                                                      //HDF file must be open when passed.   
                {
                    
                    _h5loc = H5F.open(this._sHDF_FileName, H5F.OpenMode.ACC_RDONLY);                  //todo- AVOID double open here!!! create the loc without

                    
                    if (hdfIsValidGroup(sGroupName, false))              //get to group
                    {
                        _h5g = H5G.open(_h5loc, sGroupName);

                        // Check and open dataset
                        if (hdfIsValidDataset(sDataSetName))
                        {
                            try
                            {
                                _h5d_DataArray_Double = hdfGetDoubleArrayFromGroup(sDataSetName);    // member vars dataset/space are opened in this routine.
                            }
                            catch (Exception ex)
                            {

                            }
                            
                            
                        }
                        else
                        {
                            Console.WriteLine("Invalid Dataset");
                            //todo log the issue
                        }
                    }
                    else
                    {
                        if(bWriteFails)
                            Console.WriteLine("Invalid Group - Can't find " + sGroupName);
                        //todo log the issue
                    }
                }
                else
                {
                    Console.WriteLine("Invalid File");
                    Console.ReadLine();
                }

             //   H5.Close();                    //close all
              //  ClearObjectVars();

                return _h5d_DataArray_Double;
            }

            private double[,] hdfGetDoubleArrayFromGroup(string sDataSetName)
            {
                double[,] h5d_DataArray_LOCAL;
                _h5d_Dataset = H5D.open(_h5g, sDataSetName);
                _h5d_Space = H5D.getSpace(_h5d_Dataset);
                _h5d_Type = H5D.getType(_h5d_Dataset);
                _lH5_dim = H5S.getSimpleExtentDims(_h5d_Space);
                _n_H5_DATA_ARRAY_LENGTH = Convert.ToInt32(_lH5_dim[0]);
                _n_H5_DATA_ARRAY_WIDTH = Convert.ToInt32(_lH5_dim[1]);

                h5d_DataArray_LOCAL = new double[_n_H5_DATA_ARRAY_LENGTH, _n_H5_DATA_ARRAY_WIDTH];
                H5D.read(_h5d_Dataset, new H5DataTypeId(H5T.H5Type.NATIVE_DOUBLE), new H5Array<double>(h5d_DataArray_LOCAL));
                return h5d_DataArray_LOCAL;
            }
#endregion

            #region Check / Maintenance


            public bool hdfCheckOrCreateH5(string sFile, bool bCreateIfNoExist = true)
            {
                bool bVal;
                bVal = hdfIsValidHDF(sFile);
                if (!bVal && bCreateIfNoExist)
                {
                    bVal = hdfCreateHDF(sFile);
                }
                return bVal;
            }
        
        
        // met 5/11/2013
        // 1: seems to only work with ACC_RDONLY
        // 2: important to close the file afterwards (otherwise not viewable in hdf viewer.. 

        
         public bool hdfCreateHDF(string sHDF_FilePath)
            {
                bool bReturn = true;
                if (!File.Exists(sHDF_FilePath)){
                    if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(sHDF_FilePath))) { System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(sHDF_FilePath)); }       //create dir if needed
                    _h5FileOrGroupID = H5F.create(sHDF_FilePath, H5F.CreateMode.ACC_RDONLY);
                    H5.Close();
                    _h5FileOrGroupID = null;
                }
                else{
                    bReturn = false;
                }
                return bReturn;
            }


            public bool hdfIsValidHDF(string sHDF_FilePath)
            {
                bool bReturn = true;
                if (File.Exists(sHDF_FilePath)){
                    bReturn= H5F.is_hdf5(sHDF_FilePath);
                }
                else{
                    bReturn = false;
                }
                  
                return bReturn;
            }

            private bool hdfIsValidGroup(string sGroupName, bool bCreateGroupIfEmpty)
            {
                bool bReturn = H5L.Exists(_h5FileOrGroupID, sGroupName);

                if (bCreateGroupIfEmpty && !bReturn)
                {
                    hdfCreateGroup(sGroupName);                         // todo: create group
                    bReturn = true;
                }
                return bReturn;           
            }

            private void hdfCreateGroup(string sGroup)
            {
                H5G.create(_h5loc, sGroup);
            }



            private void hdfDeleteDatasetIfExists(string sDataSetName)
            {
                if (hdfIsValidDataset(sDataSetName)){
                    H5L.Delete(_h5g,sDataSetName);

                    //todo : LOG THIS 
                }
            }
            private bool hdfIsValidDataset(string sDataSetNameOut)
            {
                bool bReturn = H5L.Exists(_h5g, sDataSetNameOut);
                return bReturn;
            }



            public void hdfClose()
            {
                H5.Close();
                _bHDF_IsOpen = false;
                ClearObjectVars();
            }



            # endregion  //read hdf5

            # region     WRITE HDF5

            public bool hdfWriteDataSeries(double[,] dArrOut, string sGroupName, string sDataSetNameOut)
            {

                bool bReturn = true;


                if (hdfCanWriteHDF())
                {
                    //  _h5FileOrGroupID = H5F.open(this._sHDF_FileName, H5F.OpenMode.ACC_RDWR);
                    _h5loc = H5F.open(this._sHDF_FileName, H5F.OpenMode.ACC_RDWR);

                    // standard case, intended for "smaller (10^1-10^2) of output. probably when we had 30k
                    if (!_bOutputArrayFormat)
                    {

                        if (hdfIsValidGroup(sGroupName, true) && dArrOut != null) //SP 14-Jul-2016 added '&& dArrOut != null
                        {
                            _h5g = H5G.open(_h5loc, sGroupName);
                            if (true)                   // met 7/7/2013- need to generate or regenerate        !hdfIsValidDataset(sDataSetNameOut))         // check for dataset  ASSUMES _h5g is set (probably want to check in the function
                            {
                                setArrayDims(ref dArrOut);
                                H5DataSpaceId spaceId = H5S.create_simple(2, _lH5_dim);
                                _h5d_TypeOut = H5T.copy(H5T.H5Type.NATIVE_DOUBLE);
                                hdfDeleteDatasetIfExists(sDataSetNameOut);
                                _h5d_Dataset = H5D.create(_h5g, sDataSetNameOut, _h5d_TypeOut, spaceId);
                            }
                            H5D.write(_h5d_Dataset, _h5d_TypeOut, new H5Array<double>(dArrOut));
                            //Console.Write("Write Successful"); //SP 7-Jun-2016 avoid printing out to console for every population
                        }
                    }
                    else    // write all data to a 2d array
                    {
                        if (hdfIsValidGroup(_sGroupName_ArrayFormat, true) && dArrOut != null) //SP 14-Jul-2016 added '&& dArrOut != null
                        {
                            _h5g = H5G.open(_h5loc, sGroupName);
                            setArrayDims(ref dArrOut);
                            H5DataSpaceId spaceId = H5S.create_simple(2, _lH5_dim);
                            _h5d_TypeOut = H5T.copy(H5T.H5Type.NATIVE_DOUBLE);
                            hdfDeleteDatasetIfExists(sDataSetNameOut);
                            _h5d_Dataset = H5D.create(_h5g, sDataSetNameOut, _h5d_TypeOut, spaceId);
                            H5D.write(_h5d_Dataset, _h5d_TypeOut, new H5Array<double>(dArrOut));
                        }
                    }
                }
                else
                {
                    bReturn = false;
                }
                return bReturn;

            }

        //utility to set the size of the dataspace
            private void setArrayDims(ref double [,] dArr)
            {
                int nRow = dArr.GetLength(0);
                int nCol = dArr.GetLength(1);
                if(!_bOutputArrayFormat)
                    _lH5_dim = new long[] { nRow, nCol };           //get a new array
                else
                    _lH5_dim = new long[] {nRow, nCol };  
            }


        //met 7/6/2013: modify to break up the file open vs file write... file must be open and write-able
            private bool hdfCanWriteHDF()
            {
                bool bCanWrite = true;

                if (this._bHDF_IsOpen && this._h5FOpenMode == H5F.OpenMode.ACC_RDWR)
                {

                }
                else
                {
                    bCanWrite = false;
                }
                return bCanWrite;
            }



    /*        private bool hdfCanWriteHDF(string sHDF_FilePath, bool bCreateHDF_IfNoFile = true)
            {
                bool bCanWrite = true;
                if (File.Exists(sHDF_FilePath))
                {
                   if (H5F.is_hdf5(sHDF_FilePath)){

                       //2 check if in a state that can be written (assume there is function for this or leave todo


                   }
                   else{
                       bCanWrite = false;
                   }
                }
                else
                {
                    if (bCreateHDF_IfNoFile)
                    {

                    }
                    else
                    {
                        bCanWrite = false;
                    }
                }
                return bCanWrite;
            }
     * 
     * */

            #endregion

        }
    }
