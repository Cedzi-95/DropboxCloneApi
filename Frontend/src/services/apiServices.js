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
    
}