import { useState, useEffect } from 'react';
import { useMsal } from '@azure/msal-react';

export const UserManagement = () => {
    const { instance } = useMsal();
    const [users, setUsers] = useState([]);
    const [error, setError] = useState(null);

    const assignRole = async (userId, role) => {
        try {
            const account = instance.getActiveAccount();
            const tokenRequest = {
                scopes: ["https://TestIndiansInIreland.onmicrosoft.com/testIIA-API/testIIA-read"],
                account: account
            };

            const tokenResponse = await instance.acquireTokenSilent(tokenRequest);
            
            const response = await fetch(`http://localhost:5005/api/users/${userId}/role`, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${tokenResponse.accessToken}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(role)
            });

            if (!response.ok) {
                throw new Error('Failed to assign role');
            }

            // Refresh user list
            fetchUsers();
        } catch (err) {
            setError(err.message);
        }
    };

    const fetchUsers = async () => {
        // Implement user fetching logic here
    };

    return (
        <div className="card mt-3">
            <div className="card-body">
                <h5 className="card-title">User Management</h5>
                {error && <div className="alert alert-danger">{error}</div>}
                <table className="table">
                    <thead>
                        <tr>
                            <th>User</th>
                            <th>Current Role</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {users.map(user => (
                            <tr key={user.id}>
                                <td>{user.displayName}</td>
                                <td>{user.role || 'User'}</td>
                                <td>
                                    <select 
                                        className="form-select"
                                        onChange={(e) => assignRole(user.id, e.target.value)}
                                        value={user.role || 'User'}
                                    >
                                        <option value="User">User</option>
                                        <option value="Admin">Admin</option>
                                    </select>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
};
