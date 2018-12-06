import { Component, OnInit } from '@angular/core';
import { User } from '../models/user';
import { Address } from '../models/address';
import { FormGroup, FormBuilder, Validators, AbstractControl, FormArray } from '@angular/forms';
import {ValidatorFn} from '../common/validatorFn';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {
  public signupFormGroup: FormGroup;
  public user: User;
  public notification: string;

  public get addresses() : FormArray {
    return <FormArray> this.signupFormGroup.get('addresses');
  }
  
  
  public addressLineValidationMessage: string ='';
  public emailValidationMessage: string = '';
  public confirmEmailValidationMessage: string = '';

  private emailValidationMessages = {email:'Please enter valid email.', 
                                    required:'Email is required.',
                                    match: 'email doesnt match.'};

  constructor(private fb: FormBuilder) { }

  ngOnInit() {
    this.setDefaultValues();
    const emailControl = this.signupFormGroup.controls.emailFormGroup.get('email');
    const confirmEmailControl = this.signupFormGroup.controls.emailFormGroup.get('confirmEmail');
    
    this.signupFormGroup.controls.emailFormGroup.valueChanges.pipe(debounceTime(5000))
    .subscribe(value=> this.setConfirmEmailValidationMesage(confirmEmailControl));

    emailControl.valueChanges.pipe(debounceTime(5000))
    .subscribe(value=> this.setEmailValidationMesage(emailControl))

    // confirmEmailControl.valueChanges.pipe(debounceTime(5000))
    // .subscribe(value=> this.setConfirmEmailValidationMesage(confirmEmailControl))

    this.signupFormGroup.get('notification').valueChanges.subscribe(      
      value=> this.setValidationForMobile(value))
  }

  public save()
  {
    this.setDefaultValues();
  }

  public setDefaultValues()
  {
    this.user = new User();
    this.user.address = new Address();
    this.notification = 'email';
    this.emailValidationMessage = '';
    this.signupFormGroup = this.fb.group({
      firstName: ['',[Validators.required]],
      lastName: ['',[Validators.required]],
      emailFormGroup : this.fb.group({
        email: ['',[Validators.required, Validators.email]],
        confirmEmail: ['',[Validators.required, Validators.email]],
      }, {validator: ValidatorFn.emailMatcherValidation('email', 'confirmEmail')}),      
      mobile: ['',[ValidatorFn.textlength(10, 13), ValidatorFn.number()]],
      blnAddAddress: true,
      notification:'',
      addresses: this.fb.array([this.addressBuilder()])
    });
    // this.signupFormGroup = this.fb.group({
    //   firstName: "",
    //   lastName: '',
    //   email: '',
    //   mobile: '',
    //   addAddress: true,
    //   addressLine: {value:'default', disabled: true}
    //});
    // this.signupFormGroup = new FormGroup({
    //     firstName: new FormControl(),
    //     lastName: new FormControl(),
    //     email: new FormControl(),
    //     mobile: new FormControl()
    // });
  }

  public addAddress(): void{
        return this.addresses.push(this.addressBuilder());
  }

  public addressBuilder(): FormGroup {
    return this.fb.group(
      {
        addressType:'1',
        addressLine:['', [ValidatorFn.text]],
        street:['',[ValidatorFn.text]],
        city: ['',[Validators.required, ValidatorFn.text]],
        state: ['',[Validators.required, ValidatorFn.text]],
        country: ['',[Validators.required,ValidatorFn.text]]
      }
    )
  }

  public reset(){
    this.setDefaultValues();
  }

  public populateTestData(){
    this.signupFormGroup.patchValue({
      firstName:"Test fisrtname ",
      lastName: "Test lastname",      
      mobile: '1234567890',
      blnAddAddress: true,
      notification:''
    })

    this.signupFormGroup.controls.emailFormGroup.patchValue({
      email:'test@test.com',
      confirmEmail:'demo'
    });
    // this.signupFormGroup.setValue({
    //   firstName:"Test fisrtname ",
    //   lastName: "Test lastname",
    //   email: "test email",
    //   mobile: "123456",
    // })
  }

  public setValidationForMobile(notifyVia: string){
    let mobileFormControl = this.signupFormGroup.get('mobile');
    if(notifyVia ==='text')
    {
      mobileFormControl.setValidators([Validators.required, ValidatorFn.textlength(10, 13), ValidatorFn.number()]);
    }else{
      mobileFormControl.clearValidators();
      mobileFormControl.setValidators([ValidatorFn.textlength(10, 13), ValidatorFn.number()]);
    }

    mobileFormControl.updateValueAndValidity();
  }

  public setEmailValidationMesage(control: AbstractControl){
    this.emailValidationMessage = '';

    if((control.touched || control.dirty || !control.valid) && control.errors){
       this.emailValidationMessage = Object.keys(control.errors).map(
            key => this.emailValidationMessage += this.emailValidationMessages[key]).join(' ');
    }
  }

  public setConfirmEmailValidationMesage(control: AbstractControl){
    this.confirmEmailValidationMessage = '';

    if((control.touched || control.dirty || !control.valid) && this.signupFormGroup.controls.emailFormGroup.errors ){
       this.confirmEmailValidationMessage = Object.keys(this.signupFormGroup.controls.emailFormGroup.errors).map(
            key => this.confirmEmailValidationMessage += this.emailValidationMessages[key]).join(' ');
    }
  }
}
