import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-premium-calculate-button',
  templateUrl: './premium-calculate-button.component.html',
  styleUrls: ['./premium-calculate-button.component.scss']
})
export class PremiumCalculateButtonComponent implements OnInit {



  readonly RESET_TIMEOUT_SEC: number = 5;

  @Input() options: number[];
  @Input() optionInProgress: number = null; // For multiple buttons
  @Input() inProgress: boolean = false; // For single button
  @Input() timeout: number = 5;
  @Input() isPreloading: boolean = false;

  @Output() optionClicked: EventEmitter<number> = new EventEmitter<number>();

  buttonText: string = "Calculate"
  error: string;
  lastOptionRated: number = null;
  bgColor: string = "btn-primary";
  buttonTooltipText: string;
  private _ratingEnabled: boolean = false;
  private _withValidModules: boolean = false;

  isErrorPreloading: boolean;
  isErrorRating: boolean;

  isSingleOnly: boolean = false;
  singleRatingSuccessful?: boolean = null;

  get buttonsDisabled() {
    return !this._ratingEnabled
      || this.optionInProgress != null
      || this.isPreloading
      || this.inProgress
      || !this._withValidModules
      || this.error != null;
  }

  get isSingleButton() {
    return (this.options?.length ?? 0) == 0;
  }

  getTitle(opt?: number): string {
    //if (this.error)
    //  return this.getError(opt);
    return this.buttonTooltipText;
  }

  constructor(
  ) {
    //console.log(this._ratingEnabled);
  }

  ngOnInit() {
    this.options = [];
    for (let i = 1; i <= this.options?.length ?? 0; i++) {
      this.options.push(i);
    }
  }

  click(option?: number) {
    if (this.isSingleButton) {
      this.inProgress = true;
    } else {
      this.lastOptionRated = null;
      this.optionInProgress = option;
    }
    this.bgColor = "btn-warning";
    this.optionClicked.emit(option);
  }

  enableRating() { this._ratingEnabled = true; }
  disableRating() { this._ratingEnabled = false; }

  getError(opt) {
    return opt === this.lastOptionRated ? this.error : null;
  }

  getBgColor(opt) {
    if (this.isSingleButton && this.singleRatingSuccessful != null) {
      if (this.singleRatingSuccessful) {
        return "btn-success";
      } else {
        return "btn-danger";
      }
    }

    if (opt === this.lastOptionRated) {
      if (this.isErrorRating == false) {
        return "btn-success";
      } else {
        return "btn-danger";
      }
    } else if (opt === this.optionInProgress) {
      return "btn-info";
    } else {
      return "btn-primary";
    }
  }

  canShowSuccess(opt) {
    if (this.isSingleButton) {
      return this.singleRatingSuccessful == true;
    } else {
      return this.lastOptionRated === opt && !(this.isErrorRating ?? true /* default to true to not show when isErrorRating is null */);
    }
  }

  canShowError(opt) {
    if (this.isSingleButton) {
      return this.singleRatingSuccessful == false;
    } else {
      return this.lastOptionRated === opt && (this.isErrorRating ?? false);
    }
  }

  /**
   * Enables/disables rating if with valid/invallid modules. 
   */
  withValidModules(isValid: boolean) {
    this._withValidModules = isValid;
    if (isValid) {
      this.enableRating();
    } else {
      this.buttonTooltipText = "There are missing required to rate fields";
      this.disableRating();
    }
  }

  /** 
   * When invoked, buttons will be disabled and a tooltip text displays 
   * indicating that the preloading is in progress.
   */
  startPreloading() {
    this.buttonTooltipText = "Preloading rater...";
    this.buttonText = "Preloading rater...";
    this.isPreloading = true;
    this.error = null;
  }

  /**
   * Enable the buttons again and clears the preloading tooltip text. 
   */
  endPreloading() {
    this.buttonTooltipText = null;
    this.isPreloading = false;
  }

  /**
   * Ends preloading but keeps the buttons disabled and show error message.
   * @param error Tooltip message displayed on buttons.
   */
  endPreloadingWithError(error: string) {
    this.error = error;
    this.buttonTooltipText = error;
    this.isPreloading = false;
  }

  completeRating() {
    this.error = null;
    this.isErrorRating = false;
    if (this.isSingleButton) {
      this.inProgress = false;
      this.singleRatingSuccessful = true;
    } else {
      this.lastOptionRated = this.optionInProgress;
      this.optionInProgress = null;
    }
    this.resetButtonState();
  }

  failRating(error: string) {
    this.error = error;
    this.isErrorRating = true;
    if (this.isSingleButton) {
      this.singleRatingSuccessful = false;
      this.inProgress = false;
    } else {
      this.lastOptionRated = this.optionInProgress;
      this.optionInProgress = null;//
    }
    this.resetButtonState();
  }

  resetButtonState() {
    setTimeout(() => {
      if (this.isSingleButton) {
        this.singleRatingSuccessful = null;
      } else {
        this.lastOptionRated = null;
      }
      this.lastOptionRated = null;
      this.buttonTooltipText = this.error;
      this.isErrorRating=  null;
    }, this.RESET_TIMEOUT_SEC * 1000);
  }

}
