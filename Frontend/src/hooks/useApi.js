import { useState, useEffect } from "react";
import apiServices from "../services/apiServices";

//Generic hook for API calls
export const useApi = (apiCall, dependencies = []) => {
    const [data, setData] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchData = async () => {
        try
        {
            setLoading(true);
            setError(null);
            const result = await apiCall();
            setData(result);
        } catch (err) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    }; 
    fetchData();
    }, dependencies);

    return { data, loading, error, refetch: () => fetchData() };
};

//Hook for folders
export const useFolders = () => {
    const [folders, setFolders] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    const fetchFolders = async () => {
        try{
            setLoading(true);
            setError(null);
            const data = await apiServices.getAllFolders();
            setFolders(data);
        } catch (err) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    };

    const createFolder = async (folderData) => {
        try {
            const newFolder = await apiServices.createFolder(folderData);
            setFolders(prev => [...prev, newFolder]);
            return newFolder;
        } catch (err) {
            setError(err.message);
            throw err;
        }
    };

    const deleteFolder = async (id) => {
        try {
            await apiServices.deleteFolder(id);
            setFolders(prev => prev.filter(folder => folder.id !== id));
        } catch (err) {
            setError(err.message)
            throw err;
        }
    };

    useEffect(() => {
        fetchFolders();
    }, []);

    return {
        folders,
        loading,
        error,
        createFolder,
        deleteFolder,
        refetch: fetchFolders
    };
};

// Hook for files in a folder
export const useFiles = (folderId) => {
    const [files, setFiles] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
  
    const fetchFiles = async () => {
      if (!folderId) return;
      
      try {
        setLoading(true);
        setError(null);
        const data = await apiService.getFilesByFolder(folderId);
        setFiles(data);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };
  
    const createFile = async (fileData) => {
      try {
        const newFile = await apiService.createFile(fileData);
        setFiles(prev => [...prev, newFile]);
        return newFile;
      } catch (err) {
        setError(err.message);
        throw err;
      }
    };
  
    const deleteFile = async (id) => {
      try {
        await apiService.deleteFile(id);
        setFiles(prev => prev.filter(file => file.id !== id));
      } catch (err) {
        setError(err.message);
        throw err;
      }
    };
  
    useEffect(() => {
      fetchFiles();
    }, [folderId]);
  
    return {
      files,
      loading,
      error,
      createFile,
      deleteFile,
      refetch: fetchFiles
    };
  };