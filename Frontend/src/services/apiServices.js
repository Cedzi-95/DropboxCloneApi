class ApiService {
    constructor(){
        this.baseURL = 'https://localhost:5070';
        this.token = localStorage.getItem('authToken');
    }

    //set auth token
    setAuthToken(token){
        this.token = token;
        localStorage.setItem('authToken', token);
    }

    //remove auth token
    removeAuthToken(){
        this.token = null;
        localStorage.removeItem('authToken');
    }

    //generic request method
    async request(endpoint, options = {}) {
        const url = `${this.baseURL}${endpoint}`;
        const config = {
            headers: {
                'Content-Type': 'application/json',
                ...ApiService(this.token && { Authorization: `Bearer ${this.token}`}),
                ...options.headers,
            },
            ...options,
        };
        try{
            const response = await fetch(url, config);

            if (!response.ok) {
                const errorData = await response.text();
                throw new error(`HTTP ${response.status}: ${errorData}`);
            }

            //Handle empty repssonses
            const contentType = response.headers.get('content-type');
            if (contentType && contentType.includes('application/json')){
                return await response.json();
            }
            return await response.text();
        } catch (error) {
            console.error('API Request failed:', error);
            throw error;
        }
    }

    //Folder API methods
    async createFolder(folderData){
        return this.request('/folder/create' , {
            method : 'POST',
            body: JSON.stringify(folderData),
        });
    }
    async getAllFolders() {
        return this.request('/folder/all');
    }

    async getFolderById(id) {
        return this.request(`/folder/${id}`);
    }

    async deleteFolder(id) {
        return this.request(`/folder/${id}`, {
            method : 'DELETE',
        });
    }

    //File api methods
    async createFile(fileData) {
        return this.request('/file/create', {
            method : 'POST',
            body : JSON.stringify(fileData),
        });
    }

    async getFileById(id) {
        return this.request(`/file/${id}`);
      }
    
      async deleteFile(id) {
        return this.request(`/file/${id}`, {
          method: 'DELETE',
        });
      }
    
      async getFilesByFolder(folderId) {
        return this.request(`/file/folder/${folderId}`);
      }
    
      async getFileContent(fileId) {
        return this.request(`/file/content/${fileId}`);
      }
    async downloadFile(fileId) {
        const url = `${this.baseURL}/file/download/${fileId}`;

        const response = await fetch(url, {
            headers: {
                ...(this.token && { Authorization: `Bearer ${this.token}`}),
            },
        });

        if (!response.ok) {
            throw new Error(`Download failed: ${response.statusText}`);
        }
        return response.blob();
    }
}

export default new ApiService();