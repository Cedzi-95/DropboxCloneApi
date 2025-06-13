// src/components/FileManager.jsx
import React, { useState } from 'react';
import { useFiles } from '../hooks/useApi';
import apiService from '../services/apiServices';

const FileManager = ({ folderId, folderName }) => {
  const { files, loading, error, createFile, deleteFile } = useFiles(folderId);
  const [showCreateForm, setShowCreateForm] = useState(false);
  const [newFileName, setNewFileName] = useState('');
  const [fileContent, setFileContent] = useState('');
  const [contentType, setContentType] = useState('text/plain');
  const [creating, setCreating] = useState(false);

  const handleCreateFile = async (e) => {
    e.preventDefault();
    if (!newFileName.trim() || !fileContent.trim()) return;

    try {
      setCreating(true);
      await createFile({
        name: newFileName,
        content: fileContent,
        contentType: contentType,
        folderId: folderId
      });
      
      setNewFileName('');
      setFileContent('');
      setShowCreateForm(false);
    } catch (err) {
      console.error('Failed to create file:', err);
    } finally {
      setCreating(false);
    }
  };

  const handleDeleteFile = async (id, name) => {
    if (window.confirm(`Are you sure you want to delete "${name}"?`)) {
      try {
        await deleteFile(id);
      } catch (err) {
        console.error('Failed to delete file:', err);
      }
    }
  };

  const handleDownloadFile = async (fileId, fileName) => {
    try {
      const blob = await apiService.downloadFile(fileId);
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = fileName;
      document.body.appendChild(a);
      a.click();
      window.URL.revokeObjectURL(url);
      document.body.removeChild(a);
    } catch (err) {
      console.error('Failed to download file:', err);
    }
  };

  const formatFileSize = (bytes) => {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
  };

  if (!folderId) {
    return <div className="file-manager">Select a folder to view files</div>;
  }

  if (loading) return <div className="loading">Loading files...</div>;
  if (error) return <div className="error">Error: {error}</div>;

  return (
    <div className="file-manager">
      <div className="file-header">
        <h2>Files in "{folderName}"</h2>
        <button 
          className="btn-primary"
          onClick={() => setShowCreateForm(true)}
        >
          Create File
        </button>
      </div>

      {showCreateForm && (
        <div className="create-form">
          <form onSubmit={handleCreateFile}>
            <input
              type="text"
              value={newFileName}
              onChange={(e) => setNewFileName(e.target.value)}
              placeholder="File name (e.g., document.txt)"
              required
            />
            
            <select 
              value={contentType} 
              onChange={(e) => setContentType(e.target.value)}
            >
              <option value="text/plain">Text File</option>
              <option value="application/json">JSON</option>
              <option value="text/html">HTML</option>
              <option value="text/css">CSS</option>
              <option value="application/javascript">JavaScript</option>
            </select>

            <textarea
              value={fileContent}
              onChange={(e) => setFileContent(e.target.value)}
              placeholder="File content..."
              rows={6}
              required
            />

            <div className="form-actions">
              <button type="submit" disabled={creating}>
                {creating ? 'Creating...' : 'Create File'}
              </button>
              <button 
                type="button" 
                onClick={() => {
                  setShowCreateForm(false);
                  setNewFileName('');
                  setFileContent('');
                }}
              >
                Cancel
              </button>
            </div>
          </form>
        </div>
      )}

      <div className="files-list">
        {files.map(file => (
          <div key={file.id} className="file-item">
            <div className="file-info">
              <div className="file-icon">📄</div>
              <div className="file-details">
                <h4>{file.name}</h4>
                <p>Type: {file.contentType}</p>
                <p>Size: {formatFileSize(file.size)}</p>
                <p>Created: {new Date(file.createdAt).toLocaleDateString()}</p>
              </div>
            </div>
            <div className="file-actions">
              <button 
                className="btn-secondary"
                onClick={() => handleDownloadFile(file.id, file.name)}
              >
                Download
              </button>
              <button 
                className="btn-danger"
                onClick={() => handleDeleteFile(file.id, file.name)}
              >
                Delete
              </button>
            </div>
          </div>
        ))}
      </div>

      {files.length === 0 && (
        <div className="empty-state">
          <p>No files in this folder yet. Create your first file!</p>
        </div>
      )}
    </div>
  );
};

export default FileManager;