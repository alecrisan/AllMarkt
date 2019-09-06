import { EventEmitter } from 'events';

class UserStore extends EventEmitter {
    constructor() {
        super();
        this._userRole = null;
    }

    get userRole() {
        return this._userRole;
    }

    set userRole(value) {
        this._userRole = value;
        this.emit('userRoleChanged');
    }
};

export default new UserStore();