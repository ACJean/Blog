const httpUtil = () => {
    const customFetch = async (endpoint, options) => {
        const defaultHeader = {
            accept: "application/json",
        };

        const controller = new AbortController();

        options.signal = controller.signal;

        options.method = options.method || "GET";
        options.headers = options.headers ? {...defaultHeader, ...options.headers} : defaultHeader;

        if (!options.body) delete options.body;

        setTimeout(() => controller.abort(), 5000);

        let dataResponse = { content: null };

        try {
            let res = await fetch(endpoint, options);

            dataResponse.ok = res.ok;
            dataResponse.status = res.status;
            dataResponse.statusText = res.statusText;
            dataResponse.url = res.url;
            dataResponse.content = await res.json().then(data => data).catch(err => null);
            return Promise.resolve(dataResponse);
        } catch (err) {
            dataResponse.ok = false;
            dataResponse.status = 0;
            dataResponse.statusText = err.message;
            dataResponse.url = endpoint;
            return Promise.reject(dataResponse);
        }
    };

    const get = (url, options = {}) => customFetch(url, options);

    const post = (url, options = {}) => {
      options.method = "POST";
      return customFetch(url, options);
    };
  
    const put = (url, options = {}) => {
      options.method = "PUT";
      return customFetch(url, options);
    };
  
    const del = (url, options = {}) => {
      options.method = "DELETE";
      return customFetch(url, options);
    };
  
    return {
      get,
      post,
      put,
      del,
    };
}

export default httpUtil