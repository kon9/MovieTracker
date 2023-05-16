import { createContext, useState, useEffect } from "react";

export const AuthContext = createContext();

export function AuthContextProvider({ children }) {
  const [authToken, setAuthToken] = useState(
    localStorage.getItem("authToken") || null
  );

  useEffect(() => {
    // whenever authToken changes, update it in localStorage
    if (authToken) {
      localStorage.setItem("authToken", authToken);
    } else {
      localStorage.removeItem("authToken");
    }
  }, [authToken]);

  return (
    <AuthContext.Provider value={{ authToken, setAuthToken }}>
      {children}
    </AuthContext.Provider>
  );
}
