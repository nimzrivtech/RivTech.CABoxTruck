import { Injectable, Input } from '@angular/core';
import { NavData } from '../../../_nav';

@Injectable({
    providedIn: 'root'
})
export class LayoutService {
    @Input() navItems: NavData[] = [];
    @Input() leftSidebarVisible: boolean = false;
    submissionSidebarVisible: boolean = true;
    navUrls: string[] = [];

    updateMenu(items: NavData[]): void {
        this.navItems = items;
        this.navUrls = this.getFlatUrls();
        this.leftSidebarVisible = true;
    }

    clearMenu(): void {
        this.navItems = [];
        this.leftSidebarVisible = false;
    }

    private getFlatUrls() {
        return this.navItems
            .reduce((a, c) => a.concat(c, c.children), [])
            .filter(n => n != undefined)
            .map(n => n.url);
    }
}
