import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class MenuOverrideService {
    overrideSideBarBehavior(): void {
        var menuDropdownOpen = document.querySelectorAll(".nav-item.nav-dropdown.open");
        menuDropdownOpen.forEach(dropdown => {
            setTimeout(function () {
                if (!dropdown.classList.contains("open"))
                    dropdown.classList.add("open");
            }, 215); // to wait for coreui js to execute first
        });
    }
}