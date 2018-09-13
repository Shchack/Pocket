import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms'
import { ToastrService } from 'ngx-toastr';   

import { AssetService } from '../../../services/asset.service';

@Component({
  selector: 'app-asset',
  templateUrl: './asset.component.html',
  styleUrls: ['./asset.component.css']
})
export class AssetComponent implements OnInit {

  constructor(public assetService : AssetService, private toastr : ToastrService) { }

  ngOnInit() {
    this.assetService.getAssetTypesList();
    this.assetService.getCurrenciesList();
    this.resetForm();
  }

  resetForm(form?: NgForm) {
    if (form != null)
      form.reset();
    this.assetService.selectedAsset = {
      id: null,
      name: '',
      typeId: '',
      currencyId: '',
      type: null,
      currency: null
    }
  }
 
  onSubmit(form: NgForm) {
    if (form.value.id == null) {
      this.assetService.postAsset(form.value)
        .subscribe(data => {
          this.resetForm(form);
          this.assetService.getAssetList();
          this.toastr.success('New Record Added Succcessfully', 'Asset');
        });
    }
    else {
      this.assetService.putAsset(form.value.id, form.value)
      .subscribe(data => {
        this.resetForm(form);
        this.assetService.getAssetList();
        this.toastr.info('Record Updated Successfully!', 'Asset');
      });
    }
  }
  
}