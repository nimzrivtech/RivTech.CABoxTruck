import { Injectable } from "@angular/core";

@Injectable()
export class StorageService {
    constructor() { }

    private get _instance(): Storage {
        return sessionStorage;
    }

    setItem(key: string, value: string) {
        if (this._instance) this._instance.setItem(key, value);
    }
    getItem(key: string) {
        return this._instance ? this._instance.getItem(key) : "";
    }
    removeItem(key: string) {
        if (this._instance) this._instance.removeItem(key);
    }

    clear() {
        this._instance.clear();
    }
}
